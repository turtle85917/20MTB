using _20MTB.Utillity;
using UnityEngine;

public class Ladle : BaseWeapon
{
    private GameObject target;

    public new void Init()
    {
        base.Init();
        animation.Play("Show");
        transform.localPosition = weaponUser.transform.position + Vector3.forward * 1.2f;
        transform.localRotation = Quaternion.identity;
        rigid.velocity = Vector2.zero;
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animation = GetComponent<Animation>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Scanner.Scan(weaponUser.transform.position, stats.Range, GameUtils.GetTargetTag(weaponUserType), out target);
        if(weaponStatus == WeaponStatus.GoAway && target == null) Reset();
        if(target != null)
        {
            weaponStatus = WeaponStatus.GoAway;
            transform.localRotation = GameUtils.LookAtTarget(transform.position, target.transform.position);
            rigid.MovePosition(Vector3.MoveTowards(rigid.position, target.transform.position, 30 * Time.deltaTime));
            sprite.flipX = 90 <= transform.rotation.eulerAngles.z && transform.rotation.eulerAngles.z <= 270;
        }
        if(weaponStatus == WeaponStatus.Idle)
        {   
            transform.localRotation *= Quaternion.AngleAxis(4, Vector3.forward);
            transform.localPosition = weaponUser.transform.position + transform.up * 1.2f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag(GameUtils.GetTargetTag(weaponUserType)) && weaponStatus == WeaponStatus.GoAway && penetrate < stats.Penetrate)
        {
            AttackManager.AttackTarget(weaponId, other.gameObject, penetrate, (affecter) => affecter.Knockback(gameObject), weaponUser);
            penetrate++;
            if(penetrate == stats.Penetrate)
            {
                gameObject.SetActive(false);
                return;
            }
            Reset();
        }
    }

    private void Reset()
    {
        target = null;
        weaponStatus = WeaponStatus.Idle;
        transform.localPosition = weaponUser.transform.position + Vector3.forward * 1.2f;
        transform.localRotation = Quaternion.identity;
    }
}
