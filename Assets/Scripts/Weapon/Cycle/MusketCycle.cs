using System.Collections;
using _20MTB.Utillity;
using UnityEngine;

public class MusketCycle : BaseCycle
{
    public override IEnumerator Cycle(GameObject weaponUser)
    {
        Weapon weapon = WeaponBundle.GetWeapon("Musket");
        GameObject musket = Object.Instantiate(
            (GameObject)weapon.weapon.resources[0],
            Player.weapons.transform,
            false
        );
        musket.name = "Musket";
        musket.transform.localPosition = Vector3.right * 0.4f;
        Musket musketScript = musket.GetComponent<Musket>();
        musketScript.stats = weapon.stats;
        musketScript.weaponUser = weaponUser;
        musketScript.weaponUserType = GameUtils.GetWeaponUserType(weaponUser);
        musketScript.Init();
        yield return null;
    }
}
