using _20MTB.Utillity;
using UnityEngine;

public class Revert : BaseWeapon
{
    public string weaponId;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag(GameUtils.GetTargetTag(weaponUserType)))
        {
            AttackManager.AttackTarget(
                weaponId, other.gameObject, penetrate,
                (affecter) => {
                    affecter.Knockback(gameObject);
                    if(weaponId == "LoveBat" && Random.value <= 0.1f)
                    {
                        affecter.Sturn();
                    }
                },
                weaponUser
            );
            penetrate++;
        }
    }
}
