using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicWandCycle : BaseCycle
{
    public override IEnumerator Cycle(GameObject weaponUser)
    {
        while(true)
        {
            Weapon weapon = WeaponBundle.GetWeaponFromTarget("MagicWand", weaponUser);
            if(weapon == null) yield break;
            yield return new WaitForSeconds(weapon.stats.Cooldown);
            var targets = new List<GameObject>(){};
            for(int i = 0; i < weapon.stats.ProjectileCount; i++)
            {
                GameObject star = ObjectPool.Get(Game.PoolManager, "Star", (GameObject)weapon.weapon.resources[0]);
                GameObject target = Scanner.Scan(Player.@object.transform.position, 10, "Enemy", targets.ToArray());
                targets.Add(target);
                Star script = star.GetComponent<Star>();
                script.target = target;
                script.stats = weapon.stats;
                script.Init();
            }
        }
    }
}
