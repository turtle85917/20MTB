using System.Collections;
using UnityEngine;

public class AxeCycle : BaseCycle
{
    public override IEnumerator Cycle(GameObject weaponUser)
    {
        while(true)
        {
            Weapon weapon = WeaponBundle.GetWeaponFromTarget("Axe", weaponUser);
            if(weapon == null) yield break;
            yield return new WaitForSeconds(weapon.stats.Cooldown);
            GameObject axe = ObjectPool.Get(Game.PoolManager, "Axe", (GameObject)weapon.weapon.resources[0]);
            axe.transform.position = weaponUser.transform.position;
            Axe script = axe.GetComponent<Axe>();
            script.stats = weapon.stats;
            script.weaponUser = weaponUser;
            script.Init();
        }
    }
}
