using System.Collections;
using UnityEngine;

public class MuayThaiCycle : BaseCycle
{
    public override IEnumerator Cycle(GameObject weaponUser)
    {
        while(true)
        {
            Weapon weapon = WeaponBundle.GetWeaponFromPlayer("MuayThai");
            yield return new WaitForSeconds(weapon.stats.Cooldown);
            if(Scanner.IsAnyTargetAround(Player.@object.transform.position, 10, "Enmey")) Player.@object.GetComponent<Affecter>().Attack(Affecter.AttackType.Once);
        }
    }
}
