using System.Collections;
using System.Linq;
using UnityEngine;

public class MuayThaiCycle : BaseCycle
{
    public override IEnumerator Cycle(GameObject weaponUser)
    {
        Weapon weapon = WeaponBundle.GetWeapon("MuayThai");
        while(true)
        {
            yield return new WaitForSeconds(weapon.stats.Cooldown);
            var enemies = Scanner.ScanAll(Player.@object.transform.position, 10, "Enemy").OrderBy(item => Vector3.Distance(item.transform.position, Player.@object.transform.position)).ToList();
            if(enemies.Count > 0)
            {
                Player.@object.GetComponent<Affecter>().AttackAnimate();
                for(int i = 0; i < weapon.stats.Penetrate; i++)
                {
                    if(enemies.Count <= i) break;
                    EnemyManager.EnemyPool enemyPool = EnemyManager.GetEnemy(enemies[i]);
                    AttackManager.AttackTarget(weapon.weapon.weaponId, enemies[i], i);
                }
            }
        }
    }
}
