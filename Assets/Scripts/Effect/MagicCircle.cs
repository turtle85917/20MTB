using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _20MTB.Stats;
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
        enemies = enemies.OrderBy(item => Vector3.Distance(item.transform.position, Game.Player.transform.position)).ToList();
        for(int i = 0; i < enemies.Count; i++)
        {
            var enemyPool = EnemyManager.GetEnemy(enemies[i]);
            Enemy script = enemyPool.target.GetComponent<Enemy>();
            // script.Sturn();
            // if(i > 0)
            //     script.Knockback(gameObject);
            EnemyManager.AttackEnemy(enemies[i], stats, i);
        }
        Destroy(gameObject);
        GameObject stamp = ObjectPool.Get(
            Game.PoolManager,
            "Stamp",
            (parent) => Instantiate(Stamp, parent.transform, false)
        );
        stamp.transform.position = target.transform.position + Vector3.up * 3;
        StopAllCoroutines();
    }
}
