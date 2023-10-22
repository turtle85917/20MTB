using System;
using UnityEngine;

public class Enemy : Affecter
{
    public GameObject text {private get; set;}
    [SerializeField] private GameObject Head;
    [Header("캐릭터 파츠")]
    [SerializeField] private SpriteRenderer headSprite;
    [SerializeField] private SpriteRenderer bodySprite;
    private EnemyManager.EnemyPool enemyPool;

    public override void Sturn(Action callbackFunc = null)
    {
        headSprite.color = Color.yellow;
        bodySprite.color = Color.yellow;
        base.Sturn(() => {
            headSprite.color = Color.white;
            bodySprite.color = Color.white;
        });
    }

    public void OnDie()
    {
        gameObject.SetActive(false);
        EnemyManager.RemoveEnemy(enemyPool);
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        enemyPool = EnemyManager.GetEnemy(gameObject);
        if(status == Status.Idle)
        {
            LookAtPlayer();
            bodySprite.flipX = transform.position.x > Player.instance.transform.position.x;
            animator.SetBool("isWalk", true);
            rigidbody.MovePosition(Vector3.MoveTowards(rigidbody.position, Player.instance.transform.position, enemyPool.moveSpeed * Time.deltaTime));
        }
        if(enemyPool.health <= 0 && status != Status.Die)
        {
            status = Status.Die;
            headSprite.flipX = false;
            bodySprite.flipX = false;
            transform.Rotate(transform.position.x > Player.instance.transform.position.x ? new Vector3(0, 180, 0) : Vector3.zero);
            animator.SetTrigger("isDie");
            Game.SpawnExpObject(transform.position, enemyPool.data.stats.Exp);
            text.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        text.transform.localPosition = (Vector2)transform.position + Vector2.down * 1.3f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && status != Status.Idle)
        {
            Game.playerData.health -= 2;
            Player.instance.Knockback(gameObject);
            TextManager.WriteDamage(other.gameObject, 2, false);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Game.playerData.health -= 1;
            TextManager.WriteDamage(other.gameObject, 1, false);
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
}
