using System.Collections;
using UnityEngine;

public class DiagumsCycle : BaseCycle
{
    public override IEnumerator Cycle(GameObject weaponUser)
    {
        Weapon weapon = WeaponBundle.GetWeapon("DiaGum");
        while(true)
        {
            yield return new WaitForSeconds(weapon.stats.Cooldown);
            GameObject diagums = ObjectPool.Get(Game.PoolManager, "Diagums", (GameObject)weapon.weapon.resources[0]);
            Diagums script = diagums.GetComponent<Diagums>();
            script.stats = weapon.stats;
            script.Init();
        }
    }
}
