using _20MTB.Utillity;
using UnityEngine;

public class Bullet : BaseWeapon
{
    public string weaponId;
    public GameObject target;

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
        if(other.CompareTag(GameUtils.GetTargetTag(weaponUserType)) && penetrate < stats.Penetrate)
        {
            AttackManager.AttackTarget(weaponId, other.gameObject, penetrate, source:weaponUser);
            penetrate++;
            if(penetrate == stats.Penetrate)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
