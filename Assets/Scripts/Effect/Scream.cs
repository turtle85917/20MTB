using System.Collections.Generic;
using UnityEngine;

public class Scream : MonoBehaviour
{
    private WeaponStats stats;
    private List<GameObject> targets;
    private SpriteRenderer spriteRenderer;

    public void Reset(WeaponStats statsVal)
    {
        stats = statsVal;
    }

    private void Awake()
    {
        targets = new(){};
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Color color = spriteRenderer.color;
        color.a -= 0.05f;
        spriteRenderer.color = color;
        transform.localScale += new Vector3(0.3f, 0.3f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy") && !targets.Contains(other.gameObject))
        {
            EnemyPool enemyPool = EnemyManager.instance.GetEnemy(other.gameObject);
            Enemy script = enemyPool.target.GetComponent<Enemy>();
            script.Knockback(Player.instance.gameObject);
            int deal = Game.instance.GetDamage(stats.Power);
            enemyPool.health -= deal;
            Damage.instance.WriteDamage(other.gameObject, deal);
            targets.Add(other.gameObject);
        }
    }

    private void OnEnable()
    {
        targets.Clear();
        Color color = spriteRenderer.color;
        color.a = 1f;
        spriteRenderer.color = color;
    }
}
