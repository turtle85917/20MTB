using System.Collections;
using UnityEngine;

public class LoveBatCycle : BaseCycle
{
    public override IEnumerator Cycle(GameObject weaponUser)
    {
        Weapon weapon = WeaponBundle.GetWeapon("LoveBat");
        while(true)
        {
            yield return new WaitForSeconds(weapon.stats.Cooldown);
            GameObject lovebat = ObjectPool.Get(Game.PoolManager, "LoveBat", (GameObject)weapon.weapon.resources[0]);
            SpinManager script = lovebat.GetComponent<SpinManager>();
            script.stats = weapon.stats;
            script.weapon = weapon;
            script.weaponUser = weaponUser;
            script.Init();
        }
    }
}
