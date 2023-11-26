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
            Player.@object.GetComponent<Affecter>().Attack(Affecter.AttackType.Repeat);
            Game.instance.StartCoroutine(Reset(weapon));
        }
    }

    private IEnumerator Reset(Weapon weapon)
    {
        yield return new WaitForSeconds(weapon.stats.Life);
        Player.@object.GetComponent<Animator>().SetInteger("AttackType", (int)Affecter.AttackType.None);
    }
}
