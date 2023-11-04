using _20MTB.Utillity;
using UnityEngine;

public class Shuriken : BaseWeapon
{
    public new void Init()
    {
        base.Init();
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rigid.AddForce(transform.right * 40);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag(GameUtils.GetTargetTag(weaponUser)))
        {
            if(weaponStatus == WeaponStatus.Idle)
            {
                weaponStatus = WeaponStatus.GoAway;
                UpdateRotation();
                rigid.velocity = Vector2.zero;
            }
            AttackManager.AttackTarget("NinjaShuriken", other.gameObject, penetrate > 0 ? 1 : 0, source:weaponUser);
            penetrate++;
        }
    }

    private void UpdateRotation()
    {
        if(90 <= transform.rotation.eulerAngles.z && transform.rotation.eulerAngles.z <= 270)
        {
            transform.rotation *= Quaternion.AngleAxis(Random.Range(180f, 270f), Vector3.forward);
        }
        else
        {
            transform.rotation *= Quaternion.AngleAxis(Random.Range(90f, 180f), Vector3.back);
        }
    }
}
