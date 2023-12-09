using System.Collections;
using System.Collections.Generic;
using _20MTB.Utillity;
using UnityEngine;

public class NoejeolCycle : BaseCycle
{
    private readonly float HEIGHT = 33.75f;

    public override IEnumerator Cycle(GameObject weaponUser)
    {
        while(true)
        {
            Weapon weapon = WeaponBundle.GetWeaponFromTarget("Noejeol", weaponUser);
			if(weapon == null) yield break;
            yield return new WaitForSeconds(weapon.stats.Cooldown);
            var targets = new List<GameObject>(){};
            for(int i = 0; i < weapon.stats.Penetrate; i++)
            {
                if(weaponUser.CompareTag("Enemy") && i == 1) break; // 플레이어는 한번만 공격
                GameObject target = Scanner.Scan(weaponUser.transform.position, weapon.stats.Range, GameUtils.GetTargetTag(weaponUser), targets.ToArray());
                if(target)
                {
                    targets.Add(target);
                    GameObject thunder = ObjectPool.Get(Game.PoolManager, "Thunder", (GameObject)weapon.weapon.resources[0]);
                    thunder.transform.position = new Vector3(target.transform.position.x, HEIGHT / 2 + target.transform.position.y);
                    Game.instance.StartCoroutine(FinisingLifeTime(weapon.stats.Life, thunder));
                    AttackManager.AttackTarget("Noejeol", target, i, (affecter) => affecter.Sturn(), weaponUser);
                }
            }
        }
    }

    private IEnumerator FinisingLifeTime(float life, GameObject thunder)
    {
        yield return new WaitForSeconds(life);
        thunder.SetActive(false);
    }
}
