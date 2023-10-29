using System.Collections;
using UnityEngine;

public class LilpaaaaaaCycle : BaseCycle
{
    public override IEnumerator Cycle(GameObject weaponUser)
    {
        Weapon weapon = WeaponBundle.GetWeapon("Lilpaaaaaa");
        while(true)
        {
            yield return new WaitForSeconds(weapon.stats.Cooldown);
            Game.instance.StartCoroutine(RepeatAttack(weapon));
            Game.PlayerObject.GetComponent<Affecter>().AttackAnimate();
        }
    }

    private IEnumerator RepeatAttack(Weapon weapon)
    {
        for(int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(i * 0.15f);
            GameObject scream = ObjectPool.Get(
                Game.PoolManager,
                "Scream",
                (parent) => Object.Instantiate((GameObject)weapon.weapon.resources[0], parent.transform, false)
            );
            scream.transform.localScale = Vector3.one * (3 - i) * 0.35f;
            scream.transform.rotation = Quaternion.AngleAxis(Random.Range(90f, -90f), Vector3.forward);
            Scream script = scream.GetComponent<Scream>();
            script.stats = weapon.stats;
            script.weaponId = weapon.weapon.weaponId;
            script.Init();
        }
    }
}
