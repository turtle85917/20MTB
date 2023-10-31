using System.Collections;
using _20MTB.Utillity;
using UnityEngine;

public class RibbonBowCycle : BaseCycle
{
    public override IEnumerator Cycle(GameObject weaponUser)
    {
        Weapon weapon = WeaponBundle.GetWeapon("RibbonBow");
        GameObject ribbonBow = Object.Instantiate(
            (GameObject)weapon.weapon.resources[0],
            GameUtils.FindGameObjectInChildWithTag(weaponUser, "Weapons").transform,
            false
        );
        ribbonBow.name = "Ribbon Bow";
        ribbonBow.transform.localPosition = Vector3.right * 0.4f;
        BulletManager script = ribbonBow.GetComponent<BulletManager>();
        script.stats = weapon.stats;
        script.weaponUser = weaponUser;
        script.weaponUserType = GameUtils.GetWeaponUserType(weaponUser);
        script.Init();
        yield return null;
    }
}
