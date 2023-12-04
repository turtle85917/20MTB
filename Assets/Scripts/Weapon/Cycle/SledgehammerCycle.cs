using System.Collections;
using _20MTB.Utillity;
using UnityEngine;

public class SledgehammerCycle : BaseCycle
{
    private readonly float RIGHT_DIR_X = 0.3f;
    private readonly float LEFT_DIR_X = -0.25f;

    public override IEnumerator Cycle(GameObject weaponUser)
    {
        while(true)
        {
            Weapon weapon = WeaponBundle.GetWeaponFromPlayer("Sledgehammer");
            yield return new WaitForSeconds(weapon.stats.Cooldown);
            GameObject sledgehammer = ObjectPool.Get(Game.PoolManager, "Sledgehammer", (GameObject)weapon.weapon.resources[0]);
            bool isRight = GameUtils.GetDirectionFromTarget(weaponUser) == 1;
            sledgehammer.GetComponent<SpriteRenderer>().flipX = isRight;
            sledgehammer.transform.position = weaponUser.transform.position + new Vector3(isRight ? RIGHT_DIR_X : LEFT_DIR_X, 1.5f);
            sledgehammer.transform.rotation = Quaternion.identity;
            Sledgehammer script = sledgehammer.GetComponent<Sledgehammer>();
            script.stats = weapon.stats;
            script.weaponUser = weaponUser;
            script.Init();
        }
    }
}
