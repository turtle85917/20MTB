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
        transform.rotation = new Quaternion(0, transform.position.x > Game.Player.transform.position.x ? 180 : 0, 0, 0);
        animator.SetBool("isWalk", true);
        if(enemyPool.health <= 0 && !animator.GetBool("isDie"))
        {
            headSprite.flipX = false;
            bodySprite.flipX = false;
            animator.SetBool("isDie", true);
            if(text != null)
            {
                text.SetActive(false);
            }
        }
    }

    private void FixedUpdate()
    {
        if(!animator.GetBool("isDie"))
        {
            rigid.MovePosition(Vector3.MoveTowards(rigid.position, Game.Player.transform.position, enemyPool.moveSpeed / 3 * Time.fixedDeltaTime));
        }
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
}
