using System.Collections;
using _20MTB.Utillity;
using UnityEngine;

public class PotatoCycle : BaseCycle
{
    public override IEnumerator Cycle(GameObject weaponUser)
    {
        while(true)
        {
            Weapon weapon = WeaponBundle.GetWeaponFromTarget("Potato", weaponUser);
			if(weapon == null) yield break;
            yield return new WaitForSeconds(weapon.stats.Cooldown);
            GameObject potato = ObjectPool.Get(Game.PoolManager, "Potato", (GameObject)weapon.weapon.resources[0]);
            Vector3 position = new Vector3(weaponUser.transform.position.x + -9.265f, weaponUser.transform.position.y + Random.Range(0.3f, Camera.main.orthographicSize + Camera.main.transform.position.y));
            if(weaponUser.CompareTag("Enemy"))
                position = weaponUser.transform.position;
            potato.transform.position = position;
            Potato script = potato.GetComponent<Potato>();
            script.stats = weapon.stats;
            script.weaponUserType = GameUtils.GetWeaponUserType(weaponUser);
            script.direction = new Vector2(Random.Range(7f, 9f), 12f);
            script.Init();
        }
    }
}
