using UnityEngine;

public class Blow : MonoBehaviour
{
    private WeaponStats stats;
    private Vector2 movement;
    private Rigidbody2D Rigidbody;
    private SpriteRenderer spriteRenderer;
    private int through;

    public void Reset(WeaponStats statsVal, Vector2 movementVal)
    {
        through = 0;
        stats = statsVal;
        movement = movementVal;
        Rigidbody.velocity = Vector2.zero;
        spriteRenderer.flipX = movement.x < 0;
        transform.localPosition = Player.instance.transform.position;
    }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(through == stats.Through)
        {
            gameObject.SetActive(false);
        }
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
            int deal = Game.instance.GetDamage(stats.Power) - through * stats.DecreasePower;
            enemy.health -= deal;
            gameObject.SetActive(false);
            Damage.instance.WriteDamage(other.gameObject, deal);
        }
    }
}
