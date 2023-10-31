using System.Collections;
using _20MTB.Utillity;
using UnityEngine;

public class LadleCycle : BaseCycle
{
    public override IEnumerator Cycle(GameObject weaponUser)
    {
        Weapon weapon = WeaponBundle.GetWeapon("Ladle");
        while(true)
        {
            yield return new WaitForSeconds(weapon.stats.Cooldown);
            GameObject ladle = ObjectPool.Get(
                Game.PoolManager,
                "Ladle",
                (GameObject)weapon.weapon.resources[0]
            );
            Ladle script = ladle.GetComponent<Ladle>();
            script.stats = weapon.stats;
            script.weaponUser = weaponUser;
            script.weaponUserType = GameUtils.GetWeaponUserType(weaponUser);
            script.Init();
        }
    }
}
