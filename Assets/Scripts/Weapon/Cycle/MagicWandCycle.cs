using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicWandCycle : BaseCycle
{
    public override IEnumerator Cycle(GameObject weaponUser)
    {
        Weapon weapon = WeaponBundle.GetWeapon("MagicWand");
        while(true)
        {
            yield return new WaitForSeconds(weapon.stats.Cooldown);
            var targets = new List<GameObject>(){};
            for(int i = 0; i < weapon.stats.ProjectileCount; i++)
            {
                GameObject star = ObjectPool.Get(
                    Game.PoolManager,
                    "Star",
                    (parent) => Object.Instantiate((GameObject)weapon.weapon.resources[0], parent.transform, false)
                );
                GameObject target = Scanner.Scan(Game.PlayerObject.transform.position, 10, "Enemy", targets.ToArray());
                targets.Add(target);
                Star script = star.GetComponent<Star>();
                script.target = target;
                script.stats = weapon.stats;
                script.weaponId = weapon.weapon.weaponId;
                script.Init();
            }
        }
    }
}
