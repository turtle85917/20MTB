using System.Collections.Generic;
using UnityEngine;

public class Scream : MonoBehaviour
{
    private WeaponStats stats;
    private int through;
    private List<GameObject> targets;
    private SpriteRenderer spriteRenderer;

    public void Reset(WeaponStats statsVal)
    {
        stats = statsVal;
        through = 0;
        targets.Clear();
        transform.localScale = new Vector3(1, 1);
        Color color = spriteRenderer.color;
        color.a = 1f;
        spriteRenderer.color = color;
    }

    private void Awake()
    {
        targets = new(){};
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Color color = spriteRenderer.color;
        color.a -= 0.08f;
        spriteRenderer.color = color;
        transform.localScale += new Vector3(0.2f, 0.2f);
        if(through == stats.Through)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy") && !targets.Contains(other.gameObject))
        {
            Game.instance.AttackEnemy(other.gameObject, stats, through, true);
            targets.Add(other.gameObject);
        }
    }
}
