using System.Collections;
using _20MTB.Utillity;
using UnityEngine;

public class WinterFlower : BaseWeapon
{
    [SerializeField] private GameObject Circle;
    private IEnumerator attackEnemyCoro;

    protected new void Init()
    {
        base.Init();
        Circle.transform.localScale = Vector3.one * stats.Range;
        Circle.GetComponent<CircleCollider2D>().radius = stats.Range / 2f;
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
                    attackEnemyCoro = AttackEnemy(other.gameObject);
                    StartCoroutine(attackEnemyCoro);
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
                    StopCoroutine(attackEnemyCoro);
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
