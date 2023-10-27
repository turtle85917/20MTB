using System.Collections;
using UnityEngine;

public class LilpaaaaaaCycle : BaseCycle
{
    public override IEnumerator Cycle(GameObject weaponUser)
    {
        Weapon weapon = WeaponBundle.GetWeapon("Lilpaaaaaa");
        while(true)
        {
            yield return new WaitForSeconds(weapon.stats.Cooldown);
            GameObject scream = ObjectPool.Get(
                Game.PoolManager,
                "Scream",
                (parent) => Object.Instantiate((GameObject)weapon.weapon.resources[0], parent.transform, false)
            );
            Scream script = scream.GetComponent<Scream>();
        }
    }
}
