using System.Collections;
using _20MTB.Utillity;
using UnityEngine;

public class DiagumsCycle : BaseCycle
{
    public override IEnumerator Cycle(GameObject weaponUser)
    {
        Weapon weapon = WeaponBundle.GetWeapon("DiaGum");
        while(true)
        {
            yield return new WaitForSeconds(weapon.stats.Cooldown);
            GameObject diagums = ObjectPool.Get(
                Game.PlayerWeapons,
                "Diagums",
                (parent) => Object.Instantiate((GameObject)weapon.weapon.resources[0], parent.transform, false)
            );
            Diagums script = diagums.GetComponent<Diagums>();
            script.weaponId = weapon.weapon.weaponId;
            script.stats = weapon.stats;
            script.weaponUser = weaponUser;
            script.weaponUserType = GameUtils.GetWeaponUserType(weaponUser);
            script.Init();
        }
    }
}
