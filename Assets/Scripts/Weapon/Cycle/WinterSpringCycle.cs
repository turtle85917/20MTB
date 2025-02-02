using System.Collections;
using UnityEngine;

public class WinterSpringCycle : BaseCycle
{
    public override IEnumerator Cycle(GameObject weaponUser)
    {
        while(true)
        {
            Weapon weapon = WeaponBundle.GetWeaponFromTarget("WinterFlower", weaponUser);
            if(weapon == null) yield break;
            yield return new WaitForSeconds(weapon.stats.Cooldown);
            GameObject winterFlower = ObjectPool.Get(Game.PoolManager, "WinterFlower", (GameObject)weapon.weapon.resources[0]);
            winterFlower.transform.position = weaponUser.transform.position + (Vector3)Random.insideUnitCircle * 5f;
            WinterFlower script = winterFlower.GetComponent<WinterFlower>();
            script.stats = weapon.stats;
            script.weaponUser = weaponUser;
            script.Init();
        }
    }
}
