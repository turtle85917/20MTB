using System.Collections;
using _20MTB.Utillity;
using UnityEngine;

public class MangnyangBeamCycle : BaseCycle
{
    public override IEnumerator Cycle(GameObject weaponUser)
    {
        Weapon weapon = WeaponBundle.GetWeapon("MangnyangBeam");
        while(true)
        {
            yield return new WaitForSeconds(weapon.stats.Cooldown);
            GameObject mnbDirector = ObjectPool.Get(Game.PoolManager, "MNB_Director", (GameObject)weapon.weapon.resources[0]);
            MangnyangBeam script = mnbDirector.GetComponent<MangnyangBeam>();
            script.stats = weapon.stats;
            script.weaponUser = weaponUser;
            script.Init();
        }
    }
}
