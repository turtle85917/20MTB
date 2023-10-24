using _20MTB.Stats;
using _20MTB.Utillity;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public WeaponStats stats {private get; set;}
    private int through;
    private new Rigidbody2D rigidbody;

    public void Reset(GameObject Maker, GameObject target)
    {
        through = 0;
        transform.position = Maker.transform.position;
        transform.rotation = GameUtils.LookAtTarget(Maker.transform.position, target.transform.position);
        rigidbody.AddForce(transform.right * stats.ProjectileSpeed, ForceMode2D.Impulse);
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy") && gameObject.activeSelf)
        {
            EnemyManager.AttackEnemy(other.gameObject, stats, through);
            through++;
            if(through == stats.Through)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
