using System.Collections;
using System.Linq;
using UnityEngine;

public class MagicCircle : BaseWeapon
{
    public GameObject target;
    [SerializeField] private GameObject Stamp;

    public new void Init()
    {
        animation.Play("Spin");
        StartCoroutine(AttackDelay());
    }

    private void Awake()
    {
        animation = GetComponent<Animation>();
    }

    private IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(stats.Life);
        var enemies = Scanner.ScanAll(Player.@object.transform.position, 4, "Enemy").OrderBy(item => Vector3.Distance(item.transform.position, Player.@object.transform.position)).ToList();
        for(int i = 0; i < enemies.Count; i++)
        {
            EnemyPool enemyPool = EnemyManager.GetEnemy(enemies[i]);
            AttackManager.AttackTarget("MagicWand", enemies[i], i, (affecter) => {
                affecter.Sturn();
                if(i > 0)
                    affecter.Knockback(Player.@object.gameObject);
            });
        }
        Destroy(gameObject);
        GameObject stamp = ObjectPool.Get(Game.PoolManager, "Stamp", Stamp);
        stamp.transform.position = target.transform.position + Vector3.up * 3;
    }
}
