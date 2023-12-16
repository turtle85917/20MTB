using System.Collections;
using _20MTB.Utillity;
using UnityEngine;

public class RibbonBowCycle : BaseCycle
{
    public override IEnumerator Cycle(GameObject weaponUser)
    {
        Weapon weapon = WeaponBundle.GetWeapon("RibbonBow");
        GameObject ribbonBow = ObjectPool.Get(GameUtils.GetWeaponsObject(weaponUser), "Ribbon Bow", (GameObject)weapon.weapon.resources[0], true);
        ribbonBow.transform.localPosition = Vector3.right * 0.4f;
        BulletManager script = ribbonBow.GetComponent<BulletManager>();
        script.stats = weapon.stats;
        script.weaponUser = weaponUser;
        script.Init();
        yield return null;
    }
}
