using System.Collections;
using _20MTB.Utillity;
using UnityEngine;

public class CexCycle : BaseCycle
{
    public override IEnumerator Cycle(GameObject weaponUser)
    {
        while(true)
        {
            Weapon weapon = WeaponBundle.GetWeaponFromTarget("Cex", weaponUser);
			if(weapon == null) yield break;
            yield return new WaitForSeconds(weapon.stats.Cooldown);
            GameObject cexMagicCircle = ObjectPool.Get(Game.PoolManager, "CexMagicCircle", (GameObject)weapon.weapon.resources[0]);
            cexMagicCircle.transform.position = weaponUser.transform.position;
            Cex script = cexMagicCircle.GetComponent<Cex>();
            script.stats = weapon.stats;
            script.weaponUser = weaponUser;
            script.weaponUserType = GameUtils.GetWeaponUserType(weaponUser);
            script.Init();
        }
    }
}
