using _20MTB.Stats;
using UnityEngine;

public class Blow : MonoBehaviour
{
    private WeaponStats stats;
    private int through;
    private Vector2 movement;
    private new Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;

    public void Reset(WeaponStats statsVal, Vector2 movementVal)
    {
        through = 0;
        stats = statsVal;
        movement = movementVal;
        spriteRenderer.flipX = movement.x < 0;
        transform.localPosition = Player.instance.transform.position;
        rigidbody.velocity = Vector2.zero;
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
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
        rigidbody.AddForce(movement * 30);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy") && gameObject.activeSelf)
        {
            EnemyManager.AttackEnemy(other.gameObject, stats, through, processFunc:(enemy) => {
                enemy.Knockback(gameObject);
            });
            through++;
        }
    }
}
