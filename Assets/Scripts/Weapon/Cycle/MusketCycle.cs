using System.Collections;
using _20MTB.Utillity;
using UnityEngine;

public class MusketCycle : BaseCycle
{
    public override IEnumerator Cycle(GameObject weaponUser)
    {
        Weapon weapon = WeaponBundle.GetWeaponFromPlayer("Musket");
        GameObject musket = Object.Instantiate(
            (GameObject)weapon.weapon.resources[0],
            GameUtils.FindGameObjectInChildWithTag(weaponUser, "Weapons").transform,
            false
        );
        musket.name = "Musket";
        musket.transform.localPosition = Vector3.right * 0.4f;
        BulletManager script = musket.GetComponent<BulletManager>();
        script.stats = weapon.stats;
        script.weaponUser = weaponUser;
        script.weaponUserType = GameUtils.GetWeaponUserType(weaponUser);
        script.Init();
        yield return null;
    }
}
