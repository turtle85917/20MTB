using System.Collections;
using UnityEngine;

public class LilpaaaaaaCycle : BaseCycle
{
    public override IEnumerator Cycle(GameObject weaponUser)
    {
        while(true)
        {
            Weapon weapon = WeaponBundle.GetWeaponFromTarget("Lilpaaaaaa", weaponUser);
            if(weapon == null) yield break;
            yield return new WaitForSeconds(weapon.stats.Cooldown);
            Player.@object.GetComponent<Affecter>().Attack(Affecter.AttackType.Repeat);
        }
    }
}
