using UnityEngine;

public class MrOmurice : BaseWeapon
{
    private Animator animator;
    private AudioSource audioSource;

    public new void Init()
    {
        audioSource.Play();
        animator.SetTrigger("Show");
    }

    public void AttackEnemies()
    {
        AudioManager.instance.PlaySound(AudioManager.SFXClip.WeaponOmurice);
        GameObject[] enemies = Scanner.ScanAll(transform.position, stats.Range, "Enemy");
        foreach(GameObject enemy in enemies)
        {
            if(!EnemyManager.IsEnemyAlive(enemy)) continue;
            AttackManager.AttackTarget("Mr.Omurice", enemy.gameObject, 0, affecter => affecter.Sturn());
        }
        if(Random.value < 0.2f && enemies.Length > 0)
            AttackManager.AttackTarget(1, Player.@object, EnemyManager.GetEnemy(enemies[Random.Range(0, enemies.Length)]));
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponentInChildren<AudioSource>();
    }

    private void Update()
    {
        transform.position = Player.@object.transform.position;
    }
}
