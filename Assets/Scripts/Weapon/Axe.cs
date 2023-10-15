using System.Collections;
using UnityEngine;

public class Axe : MonoBehaviour, IExecuteWeapon
{
    private WeaponStats stats;
    private GameObject AxePrefab;
    [SerializeField] private bool isProjectile;
    private bool goAway;
    private int through;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D Rigidbody;

    public void ExecuteWeapon(Object[] resources, WeaponStats statsVal)
    {
        stats = statsVal;
        AxePrefab = resources[0] as GameObject;
        goAway = false;
        StartCoroutine(WeaponCycle());
    }

    public void ExecuteEnemyWeapon()
    {
    }

    public void Reset(WeaponStats statsVal, GameObject target)
    {
        stats = statsVal;
        goAway = false;
        through = 0;
        Rigidbody.velocity = Vector2.zero;
        float x = 0;
        if(target.CompareTag("Enemy"))
        {
            x = (target.transform.position - Player.instance.transform.position).normalized.x;
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
        while(true)
        {
            yield return new WaitForSeconds(stats.Cooldown);
            GameObject enemy = Scanner.Scan(Player.instance.transform.position, 10, "Enemy");
            GameObject axe = ObjectPool.Get(
                Game.instance.PoolManager,
                "Axe",
                () => Instantiate(AxePrefab, Game.instance.PoolManager.transform, false)
            );
            axe.name = "Axe";
            axe.transform.position = Player.instance.transform.position;
            Axe script = axe.GetComponent<Axe>();
            script.Reset(stats, enemy ?? Player.instance.gameObject);
            yield return new WaitForSeconds(stats.Life);
            axe.SetActive(false);
        }
    }
}
