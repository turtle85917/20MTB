using _20MTB.Stats;
using _20MTB.Utillity;
using UnityEngine;

public class Blow : MonoBehaviour
{
    public int through {private get; set;}
    public WeaponStats stats {private get; set;}
    public Vector2 direction {private get; set;}
    private Vector2 movement;
    private Rigidbody2D rigid;

    public void Init()
    {
        transform.localPosition = Game.Player.transform.position;
        rigid.velocity = Vector2.zero;
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(through == stats.Penetrate)
        {
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        rigid.AddForce(direction.normalized * 10);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy") && gameObject.activeSelf)
        {
            EnemyManager.AttackEnemy(other.gameObject, stats, through, processFunc:(enemy) => {
                // enemy.Knockback(gameObject);
            });
            through++;
        }
    }
}
