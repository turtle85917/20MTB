using System.Collections;
using _20MTB.Utillity;
using UnityEngine;

public class Musket : BaseWeapon
{
    [SerializeField] private GameObject Bullet;
    private GameObject target;

    public new void Init()
    {
        StartCoroutine(BulletCycle());
    }

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Scanner.Scan(Player.@object.transform.position, stats.Range, "Enemy", out target);
        if(target != null)
        {
            sprite.flipX = false;
            transform.rotation = GameUtils.LookAtTarget(transform.position, target.transform.position);
        }
        else
        {
            sprite.flipX = GameUtils.GetDirectionFromTarget(weaponUser) == -1;
            transform.rotation = Quaternion.identity;
        }
    }

    private IEnumerator BulletCycle()
    {
        while(true)
        {
            yield return new WaitForSeconds(stats.Cooldown);
            if(target != null)
            {
                GameObject bullet = ObjectPool.Get(
                    Game.PoolManager,
                    "Bullet",
                    (parent) => Instantiate(Bullet, parent.transform, false)
                );
                Bullet script = bullet.GetComponent<Bullet>();
                script.target = target;
                script.weaponId = "Musket";
                script.stats = stats;
                script.weaponUser = weaponUser;
                script.weaponUserType = GameUtils.GetWeaponUserType(weaponUser);
                script.Init();
            }
        }
    }
}
