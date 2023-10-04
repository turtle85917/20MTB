using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MagicCircle : MonoBehaviour
{
    [SerializeField] private GameObject Stamp;
    private WeaponStats stats;
    private GameObject target;

    public void Reset(WeaponStats statsVal, GameObject targetVal)
    {
        stats = statsVal;
        target = targetVal;
        StartCoroutine(DelayAttack());
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(stats.Life);
        List<GameObject> enemies = Scanner.ScanAll(target.transform.position, 10, "Enemy", 4);
        enemies = enemies.OrderBy(item => Vector3.Distance(item.transform.position, Player.instance.transform.position)).ToList();
        for(int i = 0; i < enemies.Count; i++)
        {
            EnemyPool enemyPool = EnemyManager.instance.GetEnemy(enemies[i]);
            Enemy script = enemyPool.target.GetComponent<Enemy>();
            script.Sturn();
            if(i > 0)
                script.Knockback(gameObject);
            int deal = Game.instance.GetDamage(stats.Power) - i * stats.DecreasePower;
            enemyPool.health -= deal;
            Damage.instance.WriteDamage(enemyPool.target, deal);
        }
        Destroy(gameObject);
        GameObject stamp = ObjectPool.Get(
            Game.instance.PoolManager,
            "Stamp",
            () => Instantiate(Stamp, Game.instance.PoolManager.transform, false)
        );
        stamp.name = "Stamp";
        stamp.transform.position = target.transform.position + Vector3.up * 4;
        StopAllCoroutines();
    }
}
