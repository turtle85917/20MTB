using System.Collections;
using UnityEngine;

public class HeadpinCycle : BaseCycle
{
    public override IEnumerator Cycle(GameObject weaponUser)
    {
        Weapon weapon = WeaponBundle.GetWeapon("Headpin");
        while(true)
        {
            yield return new WaitForSeconds(weapon.stats.Cooldown);
            for(int i = 0; i < weapon.stats.Penetrate; i++)
            {
                GameObject headpin = ObjectPool.Get(
                    Game.PoolManager,
                    "Headpin",
                    (parent) => Object.Instantiate((GameObject)weapon.weapon.resources[0], parent.transform, false)
                );
                Headpin script = headpin.GetComponent<Headpin>();
                script.weaponId = weapon.weapon.weaponId;
                script.Init();
            }
        }
    }
}
