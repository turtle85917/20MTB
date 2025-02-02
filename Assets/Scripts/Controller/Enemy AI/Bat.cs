using UnityEngine;

public class Bat : EnemyAIStruct
{
    private int stack;

    protected new void Update()
    {
        if(Game.isGameOver) return;
        if(isDied) return;
        if(stack >= 3) enemyPool.health = 0;
        base.Update();
        FlipObject();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && Random.value < 0.45f)
        {
            AttackManager.AttackTarget(1, other.gameObject, enemyPool);
            AttackManager.AttackTarget(-++stack * 2, gameObject, null);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Enemy") && Random.value < 0.01f)
        {
            AttackManager.AttackTarget(1, other.gameObject, null);
            AttackManager.AttackTarget(-++stack * 2, gameObject, null);
        }
    }

    protected new void OnEnable()
    {
        base.OnEnable();
        stack = 0;
    }
}
