using _20MTB.Stats;
using _20MTB.Utillity;
using UnityEngine;

public class AxeExecute : MonoBehaviour
{
    private WeaponUseMode useMode;
    private WeaponStatus weaponStatus;
    private GameObject weaponUser;
    private WeaponStats stats;
    private new Rigidbody2D rigidbody;

    public void Reset(GameObject weaponUserVal)
    {
        stats = WeaponBundle.GetWeapon("Axe").stats;
        weaponUser = weaponUserVal;
        weaponStatus = WeaponStatus.Idle;
        transform.position = weaponUser.transform.position;
        GameObject target = Scanner.Scan(weaponUser.transform.position, 10, weaponUser.CompareTag("Player") ? "Enemy" : "Player");
        rigidbody.velocity = Vector2.zero;
        if(target != null)
        {
            rigidbody.AddForce(GetDirection(weaponUser.transform.position, target.transform.position), ForceMode2D.Impulse);
        }
        else
        {
            rigidbody.AddForce(Vector3.up * 4, ForceMode2D.Impulse);
        }
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(weaponStatus == WeaponStatus.Idle)
        {
            string targetTag = GameUtils.GetAttackTargetTag(weaponUser);
            if(other.CompareTag(targetTag))
            {
                weaponStatus = WeaponStatus.GoAway;
                if(weaponUser.CompareTag("Player"))
                {
                    EnemyManager.AttackEnemy(other.gameObject, stats, processFunc:(enemy) => {
                        // enemy.Knockback(gameObject);
                    });
                }
                if(weaponUser.CompareTag("Enemy"))
                {
                    EnemyManager.EnemyPool enemyPool = EnemyManager.GetEnemy(weaponUser);
                    int damage = CalcStat.GetDamageValueFromEnemyStat(enemyPool.data.stats.Power, stats.Power);
                    Player.playerData.health -= damage;
                    TextManager.WriteDamage(Game.Player.gameObject, damage, false);
                }
            }
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
}
