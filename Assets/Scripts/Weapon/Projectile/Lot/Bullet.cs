using _20MTB.Utillity;
using UnityEngine;

public class Bullet : BaseWeapon
{
    [HideInInspector] public string weaponId;
    [HideInInspector] public GameObject target;

    public new void Init()
    {
        base.Init();
        transform.position = weaponUser.transform.position;
    }

    private void Update()
    {
        transform.rotation = GameUtils.LookAtTarget(weaponUser.transform.position, target.transform.position);
        transform.position += (target.transform.position - weaponUser.transform.position).normalized * stats.ProjectileSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag(GameUtils.GetTargetTag(weaponUser)))
        {
            AttackManager.AttackTarget(weaponId, other.gameObject, penetrate, source:weaponUser);
            penetrate++;
        }
    }
}
