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
            for(int i = 0; i < weapon.stats.ProjectileCount; i++)
            {
                GameObject headpin = ObjectPool.Get(Game.PoolManager, "Headpin", (GameObject)weapon.weapon.resources[0]);
                Headpin script = headpin.GetComponent<Headpin>();
                script.stats = weapon.stats;
                script.Init();
            }
        }
    }
}
