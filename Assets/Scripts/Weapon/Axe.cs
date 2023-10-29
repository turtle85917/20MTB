using UnityEngine;

public class Axe : BaseWeapon
{
    public new void Init()
    {
        base.Init();
        stats = WeaponBundle.GetWeapon("Axe").stats;
        weaponStatus = WeaponStatus.Idle;
        transform.position = weaponUser.transform.position;
        rigid.velocity = Vector2.zero;
        GameObject target = Scanner.Scan(weaponUser.transform.position, 10, weaponUserType == WeaponUser.Player ? "Enemy" : "Player");
        if(target != null)
        {
            rigid.AddForce(GetDirection(weaponUser.transform.position, target.transform.position), ForceMode2D.Impulse);
        }
        else
        {
            rigid.AddForce(Vector3.up * 10, ForceMode2D.Impulse);
        }
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(weaponStatus == WeaponStatus.Idle && other.CompareTag(weaponUserType == WeaponUser.Player ? "Enemy" : "Player"))
        {
            weaponStatus = WeaponStatus.GoAway;
            AttackManager.AttackTarget("Axe", other.gameObject, penetrate, (affecter) => affecter.Knockback(gameObject), weaponUser);
        }
    }

    private Vector2 GetDirection(Vector3 targetPosition, Vector3 chaserPosition)
    {
        Vector3 distance = chaserPosition - targetPosition;
        float magnitude = distance.magnitude;
        float correction = 0.4f;
        if(chaserPosition.x < targetPosition.x)
            magnitude *= -1;
        if(weaponUserType == WeaponUser.Enemy)
            correction = 0.8f;
        return new Vector2(magnitude * correction, 18);
    }
}
