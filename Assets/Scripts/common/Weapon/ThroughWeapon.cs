using UnityEngine;

public class ThroughWeapon : MonoBehaviour
{
    protected WeaponStats stats;
    protected int through;

    protected void CheckBroken()
    {
        if(through == stats.Through)
        {
            gameObject.SetActive(false);
        }
    }

    protected EnemyPool AttackEnemy(GameObject target)
    {
        EnemyPool enemyPool = EnemyManager.instance.GetEnemy(target);
        enemyPool.target.GetComponent<Enemy>().Knockback(Player.instance.gameObject);
        int deal = Game.instance.GetDamage(stats.Power) - through * stats.DecreasePower;
        enemyPool.health -= deal;
        Damage.instance.WriteDamage(target, deal);
        through++;
        return enemyPool;
    }
}
