using System.Collections;
using _20MTB.Utillity;
using UnityEngine;

public class WakchoriCycle : BaseCycle
{
    public override IEnumerator Cycle(GameObject weaponUser)
    {
        while(true)
        {
            Weapon weapon = WeaponBundle.GetWeaponFromTarget("Wakchori", weaponUser);
            if(weapon == null) yield break;
            yield return new WaitForSeconds(weapon.stats.Cooldown);
            GameObject blow = ObjectPool.Get(Game.PoolManager, "Blow", (GameObject)weapon.weapon.resources[0]);
            Blow script = blow.GetComponent<Blow>();
            script.stats = weapon.stats;
            script.weaponUser = weaponUser;
            script.weaponUserType = GameUtils.GetWeaponUserType(weaponUser);
            script.Init();
        }
    }
}
