using System.Collections;
using UnityEngine;

public class Axe : MonoBehaviour, IExecuteWeapon
{
    private WeaponStats stats;
    private GameObject AxePrefab;
    [SerializeField] private bool isProjectile;
    private GameObject weaponUser; // 무기 사용 중인 적
    private string targetTag;
    private bool goAway;
    private int through;
    private GameObject target;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D Rigidbody;

    public void ExecuteWeapon(Object[] resources, WeaponStats statsVal)
    {
        stats = statsVal;
        AxePrefab = resources[0] as GameObject;
        goAway = false;
        StartCoroutine(WeaponCycle(false));
    }

    public void ExecuteEnemyWeapon(GameObject weaponUserVal, Object[] resources, WeaponStats statsVal)
    {
        stats = statsVal;
        AxePrefab = resources[0] as GameObject;
        goAway = false;
        weaponUser = weaponUserVal;
        StartCoroutine(WeaponCycle(true));
    }

    public void Reset(WeaponStats statsVal, GameObject targetVal, GameObject chaser, string tag)
    {
        goAway = false;
        stats = statsVal;
        through = 0;
        target = targetVal;
        targetTag = tag;
        transform.position = target.transform.position;
        Rigidbody.velocity = Vector2.zero;
        if(chaser != null)
        {
            Debug.Log($"Axe {target.name} -->> {chaser.name} :: {GetDirection(chaser.transform.position)}");
            Rigidbody.AddForce(GetDirection(chaser.transform.position), ForceMode2D.Impulse);
        }
        else
        {
            Debug.Log("Axe go-away");
            Rigidbody.AddForce(Vector3.up * 4, ForceMode2D.Impulse);
        }
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
            GameObject gameObject = Scanner.Scan(target.transform.position, 2f, targetTag);
            if(!goAway && gameObject != null)
            {
                spriteRenderer.flipX = transform.position.x < gameObject.transform.position.x;
                Rigidbody.MovePosition(Vector3.MoveTowards(Rigidbody.position, gameObject.transform.position, 40 * Time.deltaTime));
            }
            if(through == stats.Through)
            {
                goAway = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(isProjectile && !goAway)
        {
            if(target.CompareTag("Player") && other.CompareTag("Enemy"))
            {
                Game.instance.AttackEnemy(other.gameObject, stats, through, true, gameObject);
                through++;
            }
            if(target.CompareTag("Enemy") && other.CompareTag("Player"))
            {
                int deal = Game.instance.GetDamage(stats.Power);
                Player.health -= deal;
                Damage.instance.WriteDamage(other.gameObject, deal, false);
                through++;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(isProjectile)
        {
            StartCoroutine(StuckBug());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(isProjectile)
        {
            StopCoroutine(StuckBug());
        }
    }

    private Vector2 GetDirection(Vector3 chaserPosition)
    {
        Vector3 distance = chaserPosition - target.transform.position;
        float magnitude = distance.magnitude;
        float correction = 0.4f;
        if(chaserPosition.x < target.transform.position.x)
            magnitude *= -1;
        if(target.CompareTag("Enemy"))
            correction = 0.8f;
        return new Vector2(magnitude * correction, 18);
    }

    private IEnumerator WeaponCycle(bool isEnemyUsing)
    {
        target = isEnemyUsing ? weaponUser : Player.instance.gameObject;
        string enemyTag = isEnemyUsing ? "Player" : "Enemy";
        while(true)
        {
            yield return new WaitForSeconds(stats.Cooldown);
            GameObject chaser = Scanner.Scan(target.transform.position, 10, enemyTag);
            GameObject axe = ObjectPool.Get(
                Game.instance.PoolManager,
                "Axe",
                () => Instantiate(AxePrefab, Game.instance.PoolManager.transform, false)
            );
            axe.name = "Axe";
            Axe script = axe.GetComponent<Axe>();
            script.Reset(stats, target, chaser, enemyTag);
            yield return new WaitForSeconds(stats.Life);
            axe.SetActive(false);
        }
    }

    private IEnumerator StuckBug()
    {
        yield return new WaitForSeconds(0.6f);
        Rigidbody.AddForce(Vector2.up * 2, ForceMode2D.Impulse);
    }
}
