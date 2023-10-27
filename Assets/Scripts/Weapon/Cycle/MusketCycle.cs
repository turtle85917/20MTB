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
            Game.PlayerWeapons.transform,
            false
        );
        musket.name = "Musket";
        musket.transform.localPosition = Vector3.right * 0.4f;
        Musket musketScript = musket.GetComponent<Musket>();
        musketScript.Init();
        musketScript.stats = weapon.stats;
        musketScript.weaponUser = weaponUser;
        musketScript.weaponUserType = GameUtils.GetWeaponUserType(weaponUser);
        while(true)
        {
            yield return new WaitForSeconds(weapon.stats.Cooldown);
            GameObject bullet = ObjectPool.Get(
                Game.PoolManager,
                "Bullet",
                (parent) => Object.Instantiate((GameObject)weapon.weapon.resources[0], parent.transform, false)
            );
            Bullet script = bullet.GetComponent<Bullet>();
            script.weaponId = weapon.weapon.weaponId;
            script.stats = weapon.stats;
            script.weaponUser = weaponUser;
            script.weaponUserType = GameUtils.GetWeaponUserType(weaponUser);
            script.Init();
        }
    }
}
