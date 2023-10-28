using System.Collections;
using System.Linq;
using UnityEngine;

public class MagicCircle : BaseWeapon
{
    public GameObject target;
    [SerializeField] private GameObject Stamp;

    public new void Init()
    {
        StartCoroutine(AttackDelay());
    }

    private IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(stats.Life);
        var enemies = Scanner.ScanAll(Game.Player.transform.position, 4, "Enemy").OrderBy(item => Vector3.Distance(item.transform.position, Game.Player.transform.position)).ToList();
        for(int i = 0; i < enemies.Count; i++)
        {
            EnemyManager.EnemyPool enemyPool = EnemyManager.GetEnemy(enemies[i]);
            AttackManager.AttackTarget(weaponId, enemies[i], i, (affecter) => {
                affecter.Sturn();
                if(i > 0)
                    affecter.Knockback(Game.Player.gameObject);
            });
        }
        Destroy(gameObject);
        GameObject stamp = ObjectPool.Get(
            Game.PoolManager,
            "Stamp",
            (parent) => Instantiate(Stamp, parent.transform, false)
        );
        stamp.transform.position = target.transform.position + Vector3.up * 3;
    }
}
