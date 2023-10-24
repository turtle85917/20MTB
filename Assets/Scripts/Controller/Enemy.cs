using System;
using UnityEngine;

public class Enemy : BaseController
{
    public GameObject text {private get; set;} // 트위치 닉네임 텍스트
    public EnemyManager.EnemyPool enemyPool {private get; set;}

    protected override void Init()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void OnDie()
    {
        gameObject.SetActive(false);
        EnemyManager.RemoveEnemy(enemyPool);
    }

    private void Update()
    {
        LookAtPlayer();
        bodySprite.flipX = transform.position.x > Game.Player.transform.position.x;
        animator.SetBool("isWalk", true);
        // if(enemyPool.health <= 0 && status != Status.Die)
        // {
        //     status = Status.Die;
        //     headSprite.flipX = false;
        //     bodySprite.flipX = false;
        //     transform.Rotate(transform.position.x > Game.Player.transform.position.x ? new Vector3(0, 180, 0) : Vector3.zero);
        //     animator.SetTrigger("isDie");
        //     Game.SpawnExpObject(transform.position, enemyPool.data.stats.Exp);
        //     if(text != null)
        //     {
        //         text.SetActive(false);
        //     }
        // }
    }

    private void FixedUpdate()
    {
        rigid.MovePosition(Vector3.MoveTowards(rigid.position, Game.Player.transform.position, enemyPool.moveSpeed * Time.fixedDeltaTime));
    }

    private void LateUpdate()
    {
        if(text != null)
        {
            text.transform.localPosition = (Vector2)transform.position + Vector2.down * 1.3f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Player.playerData.health -= 2;
            // Game.Player.Knockback(gameObject);
            TextManager.WriteDamage(other.gameObject, 2, false);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Player.playerData.health -= 1;
            TextManager.WriteDamage(other.gameObject, 1, false);
        }
    }

    private void LookAtPlayer()
    {
        // Vector3 playerPosition = Game.Player.transform.position;
        // Vector2 distance = transform.position.x < playerPosition.x
        //     ? playerPosition - transform.position
        //     : transform.position - playerPosition
        // ;
        // float angle = (float)(Math.Atan2(distance.y, distance.x) * Mathf.Rad2Deg);
        // headSprite.flipX = transform.position.x > playerPosition.x;
        // if(Math.Abs(angle) < 80)
        //     Head.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        // else
        //     Head.transform.rotation = Quaternion.identity;
    }
}
