using System.Collections;
using System.Linq;
using _20MTB.Stats;
using UnityEngine;

public class MagicCircle : BaseWeapon
{
    [SerializeField] private GameObject Stamp;
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
        var enemies = Scanner.ScanAll(Game.Player.transform.position, 4, "Enemy").OrderBy(item => Vector3.Distance(item.transform.position, Game.Player.transform.position)).ToList();
        for(int i = 0; i < enemies.Count; i++)
        {
            var enemyPool = EnemyManager.GetEnemy(enemies[i]);
            AttackManager.AttackTarget(weaponId, enemies[i], i, (affecter) => {
                affecter.Sturn();
                if(i > 0)
                    affecter.Knockback(gameObject);
            });
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
