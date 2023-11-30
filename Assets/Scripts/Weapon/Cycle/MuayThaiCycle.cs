using System.Collections;
using UnityEngine;

public class MuayThaiCycle : BaseCycle
{
    public override IEnumerator Cycle(GameObject weaponUser)
    {
        Weapon weapon = WeaponBundle.GetWeapon("MuayThai");
        while(true)
        {
            yield return new WaitForSeconds(weapon.stats.Cooldown);
            if(Scanner.IsAnyTargetAround(Player.@object.transform.position, 10, "Enmey")) Player.@object.GetComponent<Affecter>().Attack(Affecter.AttackType.Once);
        }
    }
}
