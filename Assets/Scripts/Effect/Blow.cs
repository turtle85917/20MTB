using UnityEngine;

public class Blow : ThroughWeapon
{
    private Vector2 movement;
    private Rigidbody2D Rigidbody;
    private SpriteRenderer spriteRenderer;

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
        CheckBroken();
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
            AttackEnemy(other.gameObject);
        }
    }
}
