using System.Collections;
using UnityEngine;

public class Enemy : BaseController
{
    public GameObject text {private get; set;}                      // 트위치 닉네임 텍스트
    public EnemyManager.EnemyPool enemyPool {private get; set;}
    private Affecter affecter;
    public bool isSlowing {get; private set;}                       // 현재 느리게 걷고 있는 중인가?
    private bool isPlayerAttacking;                                 // 플레이어 도트 공격을 하는 중인가?
    private float slowRatio = 0f;                                   // 이동 속도 비율 (느리게 걷을 때만 적용됨.)

    protected override void Init()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        affecter = GetComponent<Affecter>();
    }

    protected override void OnDie()
    {
        gameObject.SetActive(false);
        EnemyManager.RemoveEnemy(enemyPool);
    }

    private void Update()
    {
        if(enemyPool.health <= 0 && !animator.GetBool("isDie"))
        {
            affecter.Reset();
            rigid.velocity = Vector2.zero;
            Game.SpawnExpObject(transform.position, enemyPool.data.stats.Exp);
            animator.SetTrigger("isDie");
            StopAllCoroutines();
            text?.SetActive(false);
        }
        else
        {
            animator.SetBool("isWalk", affecter.status == Affecter.Status.Idle);
            transform.rotation = Quaternion.AngleAxis(Player.@object.transform.position.x < transform.position.x ? 180 : 0, Vector3.up);
        }
    }

    private void FixedUpdate()
    {
        if(!animator.GetBool("isDie") && affecter.status == Affecter.Status.Idle)
        {
            rigid.MovePosition(Vector3.MoveTowards(rigid.position, Player.@object.transform.position, enemyPool.moveSpeed / 3 * (isSlowing ? slowRatio : 1) * Time.fixedDeltaTime));
        }
    }

    private void LateUpdate()
    {
        if(text != null) text.transform.localPosition = (Vector2)transform.position + Vector2.down * 1.3f;
    }

    private void OnEnable() {
        StopAllCoroutines();
        isPlayerAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 진희가 어느 편이든 무시하기
        if(other.name != "Jinhe")
        {
            if(affecter.status == Affecter.Status.Idle && other.CompareTag("Player"))
            {
                isPlayerAttacking = true;
                StartCoroutine(AttackPlayer());
            }
            if(other.CompareTag("Enemy"))
            {
                if(!other.gameObject.GetComponent<Enemy>().isSlowing)
                {
                    isSlowing = true;
                    slowRatio = Random.Range(0.5f, 0.9f);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // 진희가 어느 편이든 무시하기
        if(other.name != "Jinhe")
        {
            if(other.CompareTag("Player"))
            {
                isPlayerAttacking = false;
                StopCoroutine(AttackPlayer());
            }
            if(other.CompareTag("Enemy"))
            {
                isSlowing = false;
            }
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
