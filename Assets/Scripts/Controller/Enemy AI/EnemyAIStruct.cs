using System.Collections;
using _20MTB.Utillity;
using UnityEngine;

public class EnemyAIStruct : BaseController
{
    public GameObject text {protected get; set;}
    public EnemyPool enemyPool {protected get; set;}
    protected Affecter affecter;
    protected bool isDied;
    private IEnumerator attackPlayerCoro;

    public override void OnDie()
    {
        gameObject.SetActive(false);
        EnemyManager.RemoveEnemy(enemyPool);
        EnemyManager.DropPresent(enemyPool);
    }

    protected override void Init()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        affecter = GetComponent<Affecter>();
    }

    private void FixedUpdate()
    {
        if(Game.isGameOver) return;
        Debug.Log(affecter.status);
        if(!isDied && affecter.status == Affecter.Status.Idle)
        {
            Vector3 position = Vector3.MoveTowards(rigid.position, Player.@object.transform.position, enemyPool.moveSpeed * Time.fixedDeltaTime);
            rigid.MovePosition(GameUtils.MovePositionLimited(position));
        }
    }

    protected void Update()
    {
        if(enemyPool.health <= 0 && !isDied)
        {
            isDied = true;
            text?.SetActive(false);
            rigid.velocity = Vector2.zero;
            StopAllCoroutines();
            affecter.Reset();
            animator.SetTrigger("isDied");
        }
    }

    private void LateUpdate()
    {
        if(text != null) text.transform.localPosition = (Vector2)transform.position + Vector2.down * 1.3f;
    }

    private void OnEnable()
    {
        affecter?.Reset();
        isDied = false;
        StopAllCoroutines();
    }

    protected void OnTriggerEnter2D(Collider2D other)
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

    protected void OnTriggerExit2D(Collider2D other)
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
