using System.Collections;
using UnityEngine;

public class Axe : MonoBehaviour, IExecuteWeapon
{
    private WeaponStats stats;
    private GameObject AxePrefab;
    [SerializeField] private bool isProjectile;
    private bool isEnemyUsing;     // 적이 사용 중인가?
    private GameObject weaponUser; // 적 전용
    private bool goAway;
    private int through;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D Rigidbody;

    public void ExecuteWeapon(Object[] resources, WeaponStats statsVal)
    {
        stats = statsVal;
        AxePrefab = resources[0] as GameObject;
        goAway = false;
        isEnemyUsing = false;
        StartCoroutine(WeaponCycle());
    }

    public void ExecuteEnemyWeapon(GameObject weaponUserVal, Object[] resources, WeaponStats statsVal)
    {
        weaponUser = weaponUserVal;
        stats = statsVal;
        AxePrefab = resources[0] as GameObject;
        goAway = false;
        isEnemyUsing = true;
        StartCoroutine(WeaponCycle());
    }

    public void Reset(WeaponStats statsVal, GameObject target)
    {
        stats = statsVal;
        goAway = false;
        through = 0;
        Rigidbody.velocity = Vector2.zero;
        string targetTag = isEnemyUsing ? "Player" : "Enemy";
        float x = 0;
        if(target.CompareTag(targetTag))
        {
            x = (target.transform.position - transform.position).normalized.x;
        }
        Rigidbody.AddForce(new Vector3(x * 0.6f, 18), ForceMode2D.Impulse);
    }

    private void Awake()
    {
        if(isProjectile)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            Rigidbody = GetComponent<Rigidbody2D>();
        }
    }

    private void Update()
    {
        if(isProjectile)
        {
            // NOTE: 유도성 도끼
            GameObject enemy = Scanner.Scan(transform.position, 1.5f, "Enemy");
            if(!goAway && enemy != null)
            {
                spriteRenderer.flipX = Player.instance.transform.position.x < enemy.transform.position.x;
                Rigidbody.MovePosition(Vector3.MoveTowards(Rigidbody.position, enemy.transform.position, 40 * Time.deltaTime));
            }
            if(through == stats.Through)
            {
                goAway = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(isProjectile && !goAway && other.CompareTag("Enemy"))
        {
            Game.instance.AttackEnemy(other.gameObject, stats, through, true, gameObject);
            through++;
        }
    }

    private IEnumerator WeaponCycle()
    {
        GameObject target = isEnemyUsing ? weaponUser : Player.instance.gameObject;
        string targetTag = isEnemyUsing ? "Player" : "Enemy";
        while(true)
        {
            yield return new WaitForSeconds(stats.Cooldown);
            GameObject chaser = Scanner.Scan((isEnemyUsing ? Player.instance.gameObject : weaponUser).transform.position, 10, targetTag);
            GameObject axe = ObjectPool.Get(
                Game.instance.PoolManager,
                "Axe",
                () => Instantiate(AxePrefab, Game.instance.PoolManager.transform, false)
            );
            axe.name = "Axe";
            axe.transform.position = target.transform.position;
            Axe script = axe.GetComponent<Axe>();
            script.Reset(stats, chaser ?? target);
            yield return new WaitForSeconds(stats.Life);
            axe.SetActive(false);
        }
    }
}
