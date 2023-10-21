using System.Collections;
using _20MTB.Stats;
using _20MTB.Utillity;
using UnityEngine;

public class Axe : MonoBehaviour, IExecuteWeapon
{
    private WeaponStats stats;
    private GameObject AxePrefab;
    [SerializeField] private bool isProjectile;
    private WeaponUseMode useMode;
    private WeaponStatus weaponStatus;
    private GameObject weaponUser; // 무기 사용 중인 놈
    private int through;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D Rigidbody;

    public void ExecuteWeapon(Object[] resources, WeaponStats statsVal, GameObject useTarget)
    {
        stats = statsVal;
        AxePrefab = resources[0] as GameObject;
        useMode = useTarget == null ? WeaponUseMode.Enemy : WeaponUseMode.Player;
        weaponUser = useTarget ?? Player.instance.gameObject;
        StartCoroutine(WeaponCycle());
    }

    public void Reset(WeaponStats statsVal, ref GameObject weaponUser)
    {
        stats = statsVal;
        through = 0;
        Rigidbody.velocity = Vector2.zero;
        weaponStatus = WeaponStatus.GoAway;
        GameObject target = Scanner.Scan(weaponUser.transform.position, 10, weaponUser.CompareTag("Player") ? "Enemy" : "Player");
        if(target != null)
        {
            Rigidbody.AddForce(GetDirection(weaponUser.transform.position, target.transform.position), ForceMode2D.Impulse);
        }
        else
        {
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(isProjectile && weaponStatus == WeaponStatus.Idle)
        {
            string targetTag = GameUtils.GetAttackTargetTag(weaponUser);
            int maxCount = weaponUser ? stats.Through : stats.Count;
            if(other.CompareTag(targetTag))
            {
                if(weaponUser.CompareTag("Player"))
                {}
            }
            // if(target.CompareTag("Player") && other.CompareTag("Enemy"))
            // {
            //     EnemyManager.AttackEnemy(other.gameObject, stats, through, processFunc:(enemy) => {
            //         enemy.Knockback(gameObject);
            //     });
            //     through++;
            // }
            // if(target.CompareTag("Enemy") && other.CompareTag("Player"))
            // {
            //     int deal = Game.instance.GetDamage(stats.Power);
            //     Player.health -= deal;
            //     Damage.instance.WriteDamage(other.gameObject, deal, false);
            //     through++;
            // }
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

    private Vector2 GetDirection(Vector3 targetPosition, Vector3 chaserPosition)
    {
        Vector3 distance = chaserPosition - targetPosition;
        float magnitude = distance.magnitude;
        float correction = 0.4f;
        if(chaserPosition.x < targetPosition.x)
            magnitude *= -1;
        if(useMode == WeaponUseMode.Enemy)
            correction = 0.8f;
        return new Vector2(magnitude * correction, 18);
    }

    private IEnumerator WeaponCycle()
    {
        while(true)
        {
            yield return new WaitForSeconds(stats.Cooldown);
            GameObject axe = ObjectPool.Get(
                Game.instance.PoolManager,
                "Axe",
                () => {
                    GameObject obj = Instantiate(AxePrefab, Game.instance.PoolManager.transform, false);
                    obj.name = "Axe";
                    return obj;
                }
            );
            transform.position = weaponUser.transform.position;
            axe.GetComponent<Axe>().Reset(stats, ref weaponUser);
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
