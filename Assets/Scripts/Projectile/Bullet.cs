using _20MTB.Stats;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public WeaponStats stats {private get; set;}
    private int through;
    private new Rigidbody2D rigidbody;

    public void Reset(GameObject target)
    {
        through = 0;
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rigidbody.AddForce(transform.right * 50);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy"))
        {
            EnemyManager.AttackEnemy(other.gameObject, stats, through);
            through++;
        }
    }
}
