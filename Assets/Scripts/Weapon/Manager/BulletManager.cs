using System.Collections;
using _20MTB.Utillity;
using UnityEngine;

public class BulletManager : BaseWeapon
{
    public string weaponId;
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
        stats = WeaponBundle.GetWeaponFromTarget(weaponId, weaponUser).stats;
        Scanner.Scan(weaponUser.transform.position, stats.Range, GameUtils.GetTargetTag(weaponUser), out target);
        if(target != null)
        {
            transform.rotation = GameUtils.LookAtTarget(transform.position, target.transform.position);
            sprite.flipY = 90 < transform.rotation.eulerAngles.z && transform.rotation.eulerAngles.z < 260;
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
                script.weaponId = weaponId;
                script.weaponUser = weaponUser;
                script.Init();
                count++;
            }
        }
    }
}
