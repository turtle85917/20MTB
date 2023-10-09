using UnityEngine;

public class Diagum : ThroughWeapon
{
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
            EnemyPool enemyPool = AttackEnemy(other.gameObject);
            Enemy script = enemyPool.target.GetComponent<Enemy>();
            script.Knockback(gameObject);
        }
    }
}
