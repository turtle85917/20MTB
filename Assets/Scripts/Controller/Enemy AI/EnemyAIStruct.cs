using _20MTB.Utillity;
using UnityEngine;

public class EnemyAIStruct : BaseController
{
    public GameObject text {protected get; set;}
    public EnemyPool enemyPool {protected get; set;}
    protected Affecter affecter;
    protected bool isDied;

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
        transform.rotation = Quaternion.AngleAxis(Player.@object.transform.position.x < transform.position.x ? 180 : 0, Vector3.up);
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
}
