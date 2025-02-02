using System.Collections;
using UnityEngine;

public class StampPlumpCycle : BaseCycle
{
    public override IEnumerator Cycle(GameObject weaponUser)
    {
        while(true)
        {
            Weapon weapon = WeaponBundle.GetWeaponFromTarget("StampPlump", weaponUser);
            if(weapon == null) yield break;
            GameObject enemy = Scanner.Scan(Player.@object.transform.position, 8, "Enemy");
            if(enemy)
            {
                GameObject magicCircle = Object.Instantiate((GameObject)weapon.weapon.resources[0], enemy.transform);
                MagicCircle script = magicCircle.GetComponent<MagicCircle>();
                script.stats = weapon.stats;
                script.target = enemy;
                script.Init();
            }
            yield return new WaitForSeconds(weapon.stats.Cooldown);
        }
    }
}
