using System.Collections;
using _20MTB.Utillity;
using UnityEngine;

public class NoejeolCycle : BaseCycle
{
    private readonly int MIN_HEIGHT = 4;
    private readonly int MIN_POS_Y = 3;

    public override IEnumerator Cycle(GameObject weaponUser)
    {
        Weapon weapon = WeaponBundle.GetWeapon("Noejeol");
        while(true)
        {
            yield return new WaitForSeconds(weapon.stats.Cooldown);
            GameObject target = Scanner.Scan(weaponUser.transform.position, weapon.stats.Range, GameUtils.GetTargetTag(weaponUser));
            if(target)
            {
                GameObject thunder = ObjectPool.Get(Game.PoolManager, "Thunder", (GameObject)weapon.weapon.resources[0]);
                float newPosY = Mathf.Abs(target.transform.position.y) + 1.2f;
                thunder.GetComponent<SpriteRenderer>().size = new Vector2(1, MIN_HEIGHT + newPosY);
                thunder.transform.position = new Vector3(target.transform.position.x, MIN_POS_Y - newPosY / 2);
                Game.instance.StartCoroutine(FinisingLifeTime(weapon.stats.Life, thunder));
            }
        }
    }

    private IEnumerator FinisingLifeTime(float life, GameObject thunder)
    {
        yield return new WaitForSeconds(life);
        thunder.SetActive(false);
    }
}
