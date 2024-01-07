using _20MTB.Utillity;
using UnityEngine;

public class EnemyAIStruct : BaseController
{
    public GameObject text {protected get; set;}
    public EnemyPool enemyPool {protected get; set;}
    protected Affecter affecter;
    protected bool isDied;
    private SpriteRenderer sprite;

    public override void OnDie()
    {
        gameObject.SetActive(false);
        EnemyManager.RemoveEnemy(enemyPool);
        EnemyManager.DropPresent(enemyPool);
        Player.playerData.killCount++;
    }

    protected override void Init()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        affecter = GetComponent<Affecter>();
        sprite = GetComponent<SpriteRenderer>();
    }

    protected void Update()
    {
        if(Game.isGameOver) return;
        if(isDied) return;
        if(enemyPool == null) return;
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

    private void FixedUpdate()
    {
        if(Game.isGameOver) return;
        if(Game.cameraAgent.status == CameraAgent.Status.DieBoss) return;
        if(affecter.status != Affecter.Status.Idle) return;
        if(!isDied)
        {
            Vector3 position = Vector3.MoveTowards(rigid.position, Player.@object.transform.position, enemyPool.moveSpeed.value * Time.fixedDeltaTime);
            rigid.MovePosition(GameUtils.MovePositionLimited(position));
        }
    }

    private void LateUpdate()
    {
        if(text != null) text.transform.localPosition = (Vector2)transform.position + Vector2.down * 1.3f;
    }

    protected void OnEnable()
    {
        affecter?.Reset();
        isDied = false;
        if(sprite) sprite.color = Color.white;
        StopAllCoroutines();
    }

    protected void FlipObject() => transform.rotation = Quaternion.AngleAxis(Player.@object.transform.position.x < transform.position.x ? 180 : 0, Vector3.up);
}
