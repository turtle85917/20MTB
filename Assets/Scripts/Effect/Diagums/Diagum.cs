using UnityEngine;

public class Diagum : MonoBehaviour
{
    private WeaponStats stats;
    private int through;

    private void Awake()
    {
        Diagums script = transform.parent.GetComponent<Diagums>();
        stats = script.Stats;
    }

    private void Update()
    {
        if(through == stats.Through)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy"))
        {
            Game.instance.AttackEnemy(other.gameObject, stats, through, true);
        }
    }
}
