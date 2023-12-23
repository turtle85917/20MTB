using System.Collections;
using UnityEngine;

public class Enemy : EnemyAIStruct
{
    private IEnumerator attackPlayerCoroutine;

    protected new void Update()
    {
        if(Game.isGameOver) return;
        if(isDied) return;
        base.Update();
        FlipObject();
        animator.SetBool("isWalk", affecter.status == Affecter.Status.Idle);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 진희가 어느 편이든 무시하기
        if(other.name != "Jinhe")
        {
            if(affecter.status == Affecter.Status.Idle && other.CompareTag("Player"))
            {
                attackPlayerCoroutine = AttackPlayer();
                StartCoroutine(attackPlayerCoroutine);
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
                StopCoroutine(attackPlayerCoroutine);
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
