using UnityEngine;

public class Diagum : MonoBehaviour
{
    private WeaponStats stats;
    private int through;

    private void Awake()
    {
        Diagums script = transform.parent.GetComponent<Diagums>();
        stats = script.Stats;
    }

    private void Update()
    {
        if(through == stats.Through)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy"))
        {
            EnemyPool enemyPool = EnemyManager.instance.GetEnemy(other.gameObject);
            int deal = Game.instance.GetDamage(stats.Power) - through * stats.DecreasePower;
            enemyPool.health -= deal;
            Enemy script = enemyPool.target.GetComponent<Enemy>();
            script.Knockback(gameObject);
            Damage.instance.WriteDamage(enemyPool.target, deal);
            through++;
        }
    }
}
