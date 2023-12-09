using System.Collections;
using _20MTB.Utillity;
using UnityEngine;

public class MangnyangBeamCycle : BaseCycle
{
    public override IEnumerator Cycle(GameObject weaponUser)
    {
        while(true)
        {
            Weapon weapon = WeaponBundle.GetWeaponFromTarget("MangnyangBeam", weaponUser);
            if(weapon == null) yield break;
            yield return new WaitForSeconds(weapon.stats.Cooldown);
            GameObject mnbCore = ObjectPool.Get(Game.PoolManager, "MNB_Core", (GameObject)weapon.weapon.resources[0]);
            mnbCore.transform.position = weaponUser.transform.position;
            MNB_Core script = mnbCore.GetComponentInChildren<MNB_Core>();
            script.weaponUser = weaponUser;
        }
    }
}
