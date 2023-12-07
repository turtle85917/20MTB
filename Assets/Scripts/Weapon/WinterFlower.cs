using System.Collections;
using _20MTB.Utillity;
using UnityEngine;

public class WinterFlower : BaseWeapon
{
    [SerializeField] private GameObject Circle;

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
                    Time.timeScale = 0.6f;
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
                    Time.timeScale = 1f;
                    break;
                case "Player":
                    StopCoroutine(AttackEnemy(other.gameObject));
                    StopCoroutine(DelaySturn(other.gameObject));
                    break;
            }
        }
    }

    private IEnumerator AttackEnemy(GameObject enemy)
    {
        while(EnemyManager.IsEnemyAlive(enemy))
        {
            yield return new WaitForSeconds(0.4f);
            AttackManager.AttackTarget(3, enemy, null);
        }
    }

    private IEnumerator DelaySturn(GameObject enemy)
    {
        yield return new WaitForSeconds(10f);
        enemy.GetComponent<Affecter>().Sturn();
    }
}
