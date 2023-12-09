using System.Collections;
using UnityEngine;

public class BloodSuckingCycle : BaseCycle
{
    public override IEnumerator Cycle(GameObject weaponUser)
    {
        while(true)
        {
            Weapon weapon = WeaponBundle.GetWeaponFromTarget("BloodSucking", weaponUser);
			if(weapon == null) yield break;
            yield return new WaitForSeconds(weapon.stats.Cooldown);
            GameObject bloodSucking = ObjectPool.Get(Game.PoolManager, "BloodSucking", (GameObject)weapon.weapon.resources[0]);
            bloodSucking.transform.position = new Vector3(weaponUser.transform.position.x, Game.maxPosition.y + Camera.main.transform.position.y);
            BloodSucking script = bloodSucking.GetComponent<BloodSucking>();
            script.stats = weapon.stats;
            script.weaponUser = weaponUser;
            script.Init();
        }
    }
}
