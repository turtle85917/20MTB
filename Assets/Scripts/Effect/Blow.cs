using UnityEngine;

public class Blow : MonoBehaviour
{
    public WeaponStats stats;
    public Vector2 movement;
    private Rigidbody2D Rigidbody;
    private SpriteRenderer spriteRenderer;

    public void Reset()
    {
        Rigidbody.velocity = Vector2.zero;
        spriteRenderer.flipX = movement.x < 0;
        transform.localPosition = Player.instance.transform.position;
    }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if(gameObject.activeSelf)
        {
            Rigidbody.AddForce(movement * 30);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy"))
        {
            Enemy script = other.GetComponent<Enemy>();
            script.Knockback(Player.instance.gameObject);
            var enemy = EnemyManager.instance.GetEnemy(other.gameObject);
            int deal = stats.Power + (Game.instance.playerData.stats.Power / stats.Power);
            enemy.health -= deal;
            gameObject.SetActive(false);
            Damage.instance.WriteDamage(other.gameObject, deal);
        }
    }
}
