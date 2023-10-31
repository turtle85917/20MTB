using System.Collections;
using _20MTB.Utillity;
using UnityEngine;

public class BulletManager : BaseWeapon
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
        Scanner.Scan(weaponUser.transform.position, stats.Range, GameUtils.GetTargetTag(weaponUserType), out target);
        if(target != null)
        {
            transform.rotation = GameUtils.LookAtTarget(transform.position, target.transform.position);
            sprite.flipX = false;
            sprite.flipY = 90 <= transform.rotation.eulerAngles.z && transform.rotation.eulerAngles.z <= 270;
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
                GameObject bullet = ObjectPool.Get(Game.PoolManager, Bullet.name, Bullet);
                Bullet script = bullet.GetComponent<Bullet>();
                script.target = target;
                script.stats = stats;
                script.weaponUser = weaponUser;
                script.weaponUserType = weaponUserType;
                script.Init();
                count++;
            }
        }
    }
}
