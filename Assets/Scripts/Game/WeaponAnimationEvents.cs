using System.Collections;
using System.Linq;
using UnityEngine;

public class WeaponAnimationEvents : MonoBehaviour
{
    private Animator animator;

    public void EndAttackAnim()
    {
        animator.SetInteger("AttackType", (int)Affecter.AttackType.None);
    }

    public IEnumerator Lilpaaaaaa()
    {
        Weapon weapon = WeaponBundle.GetWeapon("Lilpaaaaaa");
        StartCoroutine(LilpaaaaaaReset());
        for(int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(i * 0.2f);
            GameObject scream = ObjectPool.Get(Game.PoolManager, "Scream", (GameObject)weapon.weapon.resources[0]);
            scream.transform.localScale = Vector3.one * i * 0.1f;
            scream.transform.rotation = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward);
            Scream script = scream.GetComponent<Scream>();
            script.stats = weapon.stats;
            script.Init();
        }

        IEnumerator LilpaaaaaaReset()
        {
            yield return new WaitForSeconds(weapon.stats.Life);
            EndAttackAnim();
        }
    }

    public void MuayThai()
    {
        Weapon weapon = WeaponBundle.GetWeapon("MuayThai");
        var enemies = Scanner.ScanAll(Player.@object.transform.position, 10, "Enemy").OrderBy(item => Vector3.Distance(item.transform.position, Player.@object.transform.position)).ToList();
        for(int i = 0; i < weapon.stats.Penetrate; i++)
        {
            if(enemies.Count <= i) break;
            EnemyPool enemyPool = EnemyManager.GetEnemy(enemies[i]);
            AttackManager.AttackTarget(weapon.weapon.weaponId, enemies[i], i);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
}
