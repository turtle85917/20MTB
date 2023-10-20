using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D Rigidbody;
    [SerializeField] private GameObject Head;
    [SerializeField] private GameObject WeaponManager;
    [Header("캐릭터 파츠")]
    [SerializeField] private SpriteRenderer headSprite;
    [SerializeField] private SpriteRenderer bodySprite;
    private EnemyPool enemyPool;
    private bool knockbacking;
    private bool sturning;
    private bool dying;
    private int forceAdd = 20;

    public IExecuteWeapon AddWeaponScript(MonoScript script)
    {
        IExecuteWeapon executeWeapon = WeaponManager.AddComponent(script.GetClass()) as IExecuteWeapon;
        return executeWeapon;
    }

    public void Knockback(GameObject target)
    {
        knockbacking = true;
        animator.SetBool("isWalk", false);
        animator.SetBool("isKnockback", true);
        Rigidbody.velocity = Vector2.zero;
        StartCoroutine(Knockbacking(target));
    }

    public void Sturn()
    {
        if(sturning || dying) return;
        animator.SetBool("isWalk", false);
        animator.SetBool("isKnockback", false);
        sturning = true;
        StartCoroutine(Sturning());
    }

    public void OnDie()
    {
        gameObject.SetActive(false);
        EnemyManager.instance.RemoveEnemy(enemyPool);
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        enemyPool = EnemyManager.instance.GetEnemy(gameObject);
        if(!(knockbacking || sturning || dying))
        {
            LookAtPlayer();
            bodySprite.flipX = transform.position.x > Player.instance.transform.position.x;
            animator.SetBool("isWalk", true);
            Rigidbody.MovePosition(Vector3.MoveTowards(Rigidbody.position, Player.instance.transform.position, enemyPool.moveSpeed * Time.deltaTime));
        }
        if(enemyPool.health <= 0 && !dying)
        {
            dying = true;
            headSprite.flipX = false;
            bodySprite.flipX = false;
            transform.Rotate(transform.position.x > Player.instance.transform.position.x ? new Vector3(0, 180, 0) : Vector3.zero);
            animator.SetTrigger("isDie");
            GameObject exp = ObjectPool.SpawnExp(transform);
            exp.GetComponent<Exp>().SetExp(enemyPool.data.stats.Exp);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !(knockbacking || sturning || dying))
        {
            Player.health -= 2;
            Player.instance.Knockback(gameObject);
            Damage.instance.WriteDamage(other.gameObject, 2, false);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Player.health -= 1;
            Damage.instance.WriteDamage(other.gameObject, 1, false);
        }
    }

    private void LookAtPlayer()
    {
        Vector3 playerPosition = Player.instance.transform.position;
        Vector2 distance = transform.position.x < playerPosition.x
            ? playerPosition - transform.position
            : transform.position - playerPosition
        ;
        float angle = (float)(Math.Atan2(distance.y, distance.x) * Mathf.Rad2Deg);
        headSprite.flipX = transform.position.x > playerPosition.x;
        if(Math.Abs(angle) < 80)
            Head.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        else
            Head.transform.rotation = Quaternion.identity;
    }

    private IEnumerator Knockbacking(GameObject target)
    {
        Vector2 direction = (transform.position - target.transform.position).normalized;
        Rigidbody.AddForce(direction * forceAdd, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.2f);
        knockbacking = false;
        Rigidbody.velocity = Vector3.zero;
        animator.SetBool("isKnockback", false);
    }

    private IEnumerator Sturning()
    {
        headSprite.color = Color.yellow;
        bodySprite.color = Color.yellow;
        yield return new WaitForSeconds(4f);
        sturning = false;
        headSprite.color = Color.white;
        bodySprite.color = Color.white;
    }
}
