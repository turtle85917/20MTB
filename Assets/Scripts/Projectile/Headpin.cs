using System.Collections;
using System.Collections.Generic;
using _20MTB.Stats;
using UnityEngine;

public class Headpin : MonoBehaviour
{
    private WeaponStats stats;
    private int through;
    private GameObject target;
    private List<GameObject> targets;
    private bool goAway;

    public void Reset(WeaponStats statsVal, GameObject targetVal, List<GameObject> targetsVal)
    {
        stats = statsVal;
        target = targetVal;
        targets = targetsVal;
        goAway = false;
        transform.rotation = Quaternion.identity;
        StartCoroutine(Hide());
    }

    private void Update()
    {
        if(goAway)
        {
            transform.position += transform.right * 15 * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.Equals(target) && !goAway)
        {
            EnemyManager.AttackEnemy(other.gameObject, stats, through);
            goAway = true;
            targets.Remove(target);
            transform.Rotate(0, 0, Random.Range(-360f, 360f));
        }
    }

    private IEnumerator Hide()
    {
        yield return new WaitForSeconds(stats.Life);
        gameObject.SetActive(false);
        targets.Remove(target);
    }
}
