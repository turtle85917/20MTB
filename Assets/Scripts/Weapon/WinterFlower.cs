using System.Collections;
using _20MTB.Utillity;
using UnityEngine;

public class WinterFlower : BaseWeapon
{
    [SerializeField] private GameObject Circle;
    private readonly float slowMoveSpeed = 0.6f;

    public new void Init()
    {
        base.Init();
        Circle.transform.localScale = Vector2.one * stats.Range;
        GetComponent<CircleCollider2D>().radius = stats.Range / 2f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag(GameUtils.GetTargetTag(weaponUser)))
        {
            switch(weaponUser.tag)
            {
                case "Enemy":
                    Player.playerData.moveSpeed.otherMoveSpeed -= slowMoveSpeed;
                    StartCoroutine(AttackPlayer());
                    break;
                case "Player":
                    StartCoroutine(AttackEnemy(other.gameObject));
                    StartCoroutine(DelaySturn(other.gameObject));
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag(GameUtils.GetTargetTag(weaponUser)))
        {
            switch(weaponUser.tag)
            {
                case "Enemy":
                    Player.playerData.moveSpeed.otherMoveSpeed += slowMoveSpeed;
                    StopCoroutine(AttackPlayer());
                    break;
                case "Player":
                    EnemyManager.GetEnemy(other.gameObject).moveSpeed.otherMoveSpeed += slowMoveSpeed;
                    StopCoroutine(AttackEnemy(other.gameObject));
                    StopCoroutine(DelaySturn(other.gameObject));
                    break;
            }
        }
    }

    private IEnumerator AttackEnemy(GameObject enemy)
    {
        EnemyPool enemyPool = EnemyManager.GetEnemy(enemy);
        while(EnemyManager.IsEnemyAlive(enemy))
        {
            yield return new WaitForSeconds(0.4f);
            enemyPool.moveSpeed.otherMoveSpeed -= slowMoveSpeed;
            AttackManager.AttackTarget(3, enemy, null);
        }
    }

    private IEnumerator AttackPlayer()
    {
        while(!Game.isGameOver)
        {
            yield return new WaitForSeconds(0.8f);
            AttackManager.AttackTarget(4, Player.@object, EnemyManager.GetEnemy(weaponUser));
        }
    }

    private IEnumerator DelaySturn(GameObject enemy)
    {
        yield return new WaitForSeconds(10f);
        enemy.GetComponent<Affecter>().Sturn();
    }
}
