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
                GameObject target;
                Scanner.Scan(Game.Player.transform.position, 6, "Enemy", out target);
                while(targets.Contains(target))
                {
                    Scanner.Scan(Game.Player.transform.position, 6, "Enemy", out target);
                }
                targets.Add(target);
                Star script = star.GetComponent<Star>();
                script.target = target;
            }
        }
    }
}
