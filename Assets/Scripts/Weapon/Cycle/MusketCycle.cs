using System.Collections;
using _20MTB.Utillity;
using UnityEngine;

public class MusketCycle : BaseCycle
{
    public override IEnumerator Cycle(GameObject weaponUser)
    {
        Weapon weapon = WeaponBundle.GetWeaponFromTarget("Musket", weaponUser);
            if(weapon == null) yield break;
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
        script.Init();
        yield return null;
    }
}
