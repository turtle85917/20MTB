using System.Collections;
using UnityEngine;

public class MrOmuriceCycle : BaseCycle
{
    public override IEnumerator Cycle(GameObject weaponUser)
    {
        while(true)
        {
            Weapon weapon = WeaponBundle.GetWeaponFromTarget("Mr.Omurice", weaponUser);
            if(weapon == null) yield break;
            yield return new WaitForSeconds(weapon.stats.Cooldown);
            GameObject mrOmurice = ObjectPool.Get(Game.PoolManager, "MrOmurice", (GameObject)weapon.weapon.resources[0]);
            mrOmurice.transform.position = Player.@object.transform.position;
            MrOmurice script = mrOmurice.GetComponent<MrOmurice>();
            script.stats = weapon.stats;
            script.Init();
        }
    }
}
