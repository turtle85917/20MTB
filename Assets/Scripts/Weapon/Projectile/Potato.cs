using _20MTB.Utillity;
using UnityEngine;

public class Potato : BaseWeapon
{
    public Vector2 direction;

    public new void Init()
    {
        base.Init();
        rigid.AddForce(direction, ForceMode2D.Impulse);
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(weaponStatus == WeaponStatus.Idle)
        {
            var targets = Scanner.ScanAll(transform.position, stats.Range, GameUtils.GetTargetTag(weaponUserType), stats.Penetrate);
            if(targets.Count > 0)
            {
                rigid.velocity = Vector2.zero;
                weaponStatus = WeaponStatus.GoAway;
                foreach(GameObject target in targets)
                {
                    AttackManager.AttackTarget("Potato", target, 0, (affecter) => affecter.Knockback(gameObject), weaponUser);
                }
            }
        }
    }
}
