using System.Collections;
using UnityEngine;

public class Star : BaseWeapon
{
    public GameObject target;
    private Vector2 movement;
    private readonly Color[] colors = new Color[4]{
        new Color(0.8f, 0.3f, 0.3f),
        new Color(0.3f, 0.8f, 0.5f),
        new Color(0.3f, 0.4f, 0.8f),
        new Color(0.8f, 0.8f, 0.3f)
    };

    public new void Init()
    {
        base.Init();
        rigid.velocity = Vector2.zero;
        sprite.color = colors[Random.Range(0, colors.Length)];
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(weaponStatus == WeaponStatus.GoAway)
        {
            rigid.AddForce(movement * 20);
            return;
        }
        if(target)
        {
            rigid.MovePosition(Vector3.MoveTowards(rigid.position, target.transform.position, 14 * Time.deltaTime));
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(target == null) return;
        if(target.Equals(other.gameObject))
        {
            StartCoroutine(AttackEnemy());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(target == null) return;
        if(target.Equals(other.gameObject))
        {
            StopCoroutine(AttackEnemy());
        }
    }

    private IEnumerator AttackEnemy()
    {
        while(target || penetrate < stats.Penetrate)
        {
            yield return new WaitForSeconds(0.5f);
            AttackManager.AttackTarget(weaponId, target, penetrate);
            var enemyPool = EnemyManager.GetEnemy(target);
            if(enemyPool.health <= 0)
            {
                target = null;
                StopAllCoroutines();
            }
            penetrate++;
        }
        gameObject.SetActive(false);
    }
}
