using System.Collections;
using UnityEngine;

public class Enemy : BaseController
{
    public GameObject text {private get; set;} // 트위치 닉네임 텍스트
    public EnemyManager.EnemyPool enemyPool {private get; set;}
    private Affecter affecter;
    private bool isPlayerAttacking;

    protected override void Init()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        affecter = GetComponent<Affecter>();
    }

    public void OnDie()
    {
        gameObject.SetActive(false);
        EnemyManager.RemoveEnemy(enemyPool);
    }

    private void Update()
    {
        transform.rotation = Quaternion.AngleAxis(transform.position.x > Player.@object.transform.position.x ? 180 : 0, Vector3.up);
        animator.SetBool("isWalk", affecter.status == Affecter.Status.Idle);
        if(enemyPool.health <= 0 && !animator.GetBool("isDie"))
        {
            headSprite.flipX = false;
            bodySprite.flipX = false;
            rigid.velocity = Vector2.zero;
            animator.SetBool("isDie", true);
            StopAllCoroutines();
            affecter.Reset();
            if(text != null)
            {
                text.SetActive(false);
            }
        }
    }

    private void FixedUpdate()
    {
        if(!animator.GetBool("isDie") && affecter.status == Affecter.Status.Idle)
        {
            rigid.MovePosition(Vector3.MoveTowards(rigid.position, Player.@object.transform.position, enemyPool.moveSpeed / 3 * Time.fixedDeltaTime));
        }
    }

    private void LateUpdate()
    {
        if(text != null)
        {
            text.transform.localPosition = (Vector2)transform.position + Vector2.down * 1.3f;
        }
    }

    private void OnEnable() {
        StopAllCoroutines();
        isPlayerAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(affecter.status == Affecter.Status.Idle && other.CompareTag("Player"))
        {
            isPlayerAttacking = true;
            StartCoroutine(AttackPlayer());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            isPlayerAttacking = false;
            StopCoroutine(AttackPlayer());
        }
    }

    private IEnumerator AttackPlayer()
    {
        while(isPlayerAttacking)
        {
            yield return new WaitForSeconds(0.3f);
            Player.playerData.health -= 2;
            TextManager.WriteDamage(Player.@object, 2, false);
        }
        yield break;
    }
}
