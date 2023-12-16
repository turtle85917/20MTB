using System.Collections;
using _20MTB.Utillity;
using UnityEngine;

public class MusketCycle : BaseCycle
{
    public override IEnumerator Cycle(GameObject weaponUser)
    {
        Weapon weapon = WeaponBundle.GetWeaponFromTarget("Musket", weaponUser);
        if(weapon == null) yield break;
        GameObject musket = ObjectPool.Get(GameUtils.GetWeaponsObject(weaponUser), "Musket", (GameObject)weapon.weapon.resources[0], true);
        musket.transform.localPosition = Vector3.left * 0.4f;
        BulletManager script = musket.GetComponent<BulletManager>();
        script.stats = weapon.stats;
        script.weaponUser = weaponUser;
        script.Init();
        yield return null;
    }
}
