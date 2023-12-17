using System.Collections;
using _20MTB.Utillity;
using UnityEngine;

public class Enemy : BaseController
{
    public GameObject text {private get; set;}                      // 트위치 닉네임 텍스트
    public EnemyPool enemyPool {private get; set;}
    private Affecter affecter;
    private bool isDied;                                            // 죽은 상태인가?
    private IEnumerator attackPlayerCoro;

    protected override void Init()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        affecter = GetComponent<Affecter>();
    }

    public override void OnDie()
    {
        gameObject.SetActive(false);
        EnemyManager.RemoveEnemy(enemyPool);
        EnemyManager.DropPresent(enemyPool);
    }

    private void Update()
    {
        if(Game.isGameOver) return;
        if(enemyPool.health <= 0 && !isDied)
        {
            isDied = true;
            text?.SetActive(false);
            rigid.velocity = Vector2.zero;
            StopAllCoroutines();
            affecter.Reset();
            animator.SetTrigger("isDied");
        }
        else
        {
            animator.SetBool("isWalk", affecter.status == Affecter.Status.Idle);
            transform.rotation = Quaternion.AngleAxis(Player.@object.transform.position.x < transform.position.x ? 180 : 0, Vector3.up);
        }
    }

    private void FixedUpdate()
    {
        if(Game.isGameOver) return;
        if(!isDied && affecter.status == Affecter.Status.Idle)
        {
            Vector3 position = Vector3.MoveTowards(rigid.position, Player.@object.transform.position, enemyPool.moveSpeed * Time.fixedDeltaTime);
            rigid.MovePosition(GameUtils.MovePositionLimited(position));
        }
    }

    private void LateUpdate()
    {
        if(text != null) text.transform.localPosition = (Vector2)transform.position + Vector2.down * 1.3f;
    }

    private void OnEnable() {
        StopAllCoroutines();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 진희가 어느 편이든 무시하기
        if(other.name != "Jinhe")
        {
            if(affecter.status == Affecter.Status.Idle && other.CompareTag("Player"))
            {
                attackPlayerCoro = AttackPlayer();
                StartCoroutine(attackPlayerCoro);
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
                StopCoroutine(attackPlayerCoro);
            }
        }
    }

    private IEnumerator AttackPlayer()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.3f);
            AttackManager.AttackTarget(2, Player.@object, enemyPool);
        }
    }
}
