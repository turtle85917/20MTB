using System.Collections;
using System.Collections.Generic;
using _20MTB.Stats;
using UnityEngine;

public class Star : MonoBehaviour
{
    private WeaponStats stats;
    private GameObject target;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D Rigidbody;
    private List<GameObject> targets;
    private bool goAway;
    private int through;
    private Vector2 movement;
    private readonly Color[] colors = new Color[4]{
        new Color(0.8f, 0.3f, 0.3f),
        new Color(0.3f, 0.8f, 0.5f),
        new Color(0.3f, 0.4f, 0.8f),
        new Color(0.8f, 0.8f, 0.3f)
    };

    public void Reset(WeaponStats statsVal, List<GameObject> targetsVal)
    {
        stats = statsVal;
        targets = targetsVal;
        through = 0;
        target = null;
        Rigidbody.velocity = Vector2.zero;
        StartCoroutine(Hide());
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(goAway)
        {
            Rigidbody.AddForce(movement * 20);
            return;
        }
        if(target == null && !goAway)
        {
            GameObject enemy = Scanner.ScanFilter(transform.position, 10, "Enemy", targets);
            if(enemy == null)
            {
                movement = Player.lastDirection;
                goAway = true;
            }
            else
            {
                target = enemy;
                targets.Add(target);
            }
        }
        if(target)
        {
            Rigidbody.MovePosition(Vector3.MoveTowards(Rigidbody.position, target.transform.position, 14 * Time.deltaTime));
        }
        if(through == stats.Penetrate)
        {
            StopAllCoroutines();
            gameObject.SetActive(false);
            targets.Remove(target);
        }
    }

    private void OnEnable()
    {
        spriteRenderer.color = colors[Random.Range(0, colors.Length)];
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(target == null) return;
        if(target.Equals(other.gameObject))
        {
            StartCoroutine(AttackEnemy());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(target == null) return;
        if(target.Equals(other.gameObject))
        {
            StopCoroutine(AttackEnemy());
        }
    }

    private IEnumerator AttackEnemy()
    {
        while(target)
        {
            yield return new WaitForSeconds(0.5f);
            EnemyManager.AttackEnemy(target, stats, through);
            var enemyPool = EnemyManager.GetEnemy(target);
            if(enemyPool.health <= 0)
                targets.Remove(target);
            through++;
        }
    }

    private IEnumerator Hide()
    {
        yield return new WaitForSeconds(stats.Life);
        gameObject.SetActive(false);
        targets.Remove(target);
    }
}
