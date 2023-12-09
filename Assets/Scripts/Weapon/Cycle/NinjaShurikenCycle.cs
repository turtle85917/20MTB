using System.Collections;
using _20MTB.Utillity;
using UnityEngine;

public class NinjaShurikenCycle : BaseCycle
{
    public override IEnumerator Cycle(GameObject weaponUser)
    {
        while(true)
        {
            Weapon weapon = WeaponBundle.GetWeaponFromTarget("NinjaShuriken", weaponUser);
            if(weapon == null) yield break;
            yield return new WaitForSeconds(weapon.stats.Cooldown);
            GameObject target = Scanner.Scan(weaponUser.transform.position, 4, GameUtils.GetTargetTag(weaponUser));
            if(target)
            {
                GameObject shuriken = ObjectPool.Get(Game.PoolManager, "Shuriken", (GameObject)weapon.weapon.resources[0]);
                shuriken.transform.rotation = GameUtils.LookAtTarget(weaponUser.transform.position, target.transform.position);
                shuriken.transform.position = weaponUser.transform.position;
                Shuriken script = shuriken.GetComponent<Shuriken>();
                script.stats = weapon.stats;
                script.weaponUser = weaponUser;
                script.Init();
            }
        }
    }
}
