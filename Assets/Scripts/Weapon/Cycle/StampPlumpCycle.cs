using System.Collections;
using UnityEngine;

public class StampPlumpCycle : BaseCycle
{
    public override IEnumerator Cycle(GameObject weaponUser)
    {
        Weapon weapon = WeaponBundle.GetWeapon("StampPlump");
        while(true)
        {
            GameObject enemy = Scanner.Scan(Game.Player.transform.position, 8, "Enemy");
            if(enemy)
            {
                GameObject magicCircle = Object.Instantiate((GameObject)weapon.weapon.resources[0], enemy.transform);
                MagicCircle script = magicCircle.GetComponent<MagicCircle>();
                script.weaponId = weapon.weapon.weaponId;
                script.stats = weapon.stats;
                script.target = enemy;
                script.Init();
            }
            yield return new WaitForSeconds(weapon.stats.Cooldown);
        }
    }
}
