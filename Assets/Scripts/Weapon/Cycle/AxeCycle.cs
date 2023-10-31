using System.Collections;
using _20MTB.Utillity;
using UnityEngine;

public class AxeCycle : BaseCycle
{
    public override IEnumerator Cycle(GameObject weaponUser)
    {
        Weapon weapon = WeaponBundle.GetWeapon("Axe");
        while(true)
        {
            yield return new WaitForSeconds(weapon.stats.Cooldown);
            GameObject axe = ObjectPool.Get(Game.PoolManager, "Axe", (GameObject)weapon.weapon.resources[0]);
            axe.transform.position = weaponUser.transform.position;
            Axe script = axe.GetComponent<Axe>();
            script.stats = weapon.stats;
            script.weaponUser = weaponUser;
            script.weaponUserType = GameUtils.GetWeaponUserType(weaponUser);
            script.Init();
        }
    }
}
