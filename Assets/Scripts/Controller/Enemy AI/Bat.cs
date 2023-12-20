using UnityEngine;

public class Bat : EnemyAIStruct
{
    private int stack;

    protected override void Init()
    {
        base.Init();
        stack = 0;
    }

    protected new void Update()
    {
        if(Game.isGameOver) return;
        if(isDied) return;
        if(stack > 6)
        {
            enemyPool.health = 0;
            return;
        }
        base.Update();
        animator.SetBool("isWalk", affecter.status == Affecter.Status.Idle);
        transform.rotation = Quaternion.AngleAxis(Player.@object.transform.position.x < transform.position.x ? 180 : 0, Vector3.up);
    }

    protected new void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if(other.CompareTag("Player"))
        {
            AttackManager.AttackTarget(++stack, other.gameObject, enemyPool);
            AttackManager.AttackTarget(-stack, gameObject, null);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            AttackManager.AttackTarget(++stack, other.gameObject, null);
            AttackManager.AttackTarget(-stack, gameObject, null);
        }
    }
}
