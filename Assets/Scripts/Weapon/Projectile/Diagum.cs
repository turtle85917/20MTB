using UnityEngine;

public class Diagum : BaseWeapon
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy"))
        {
            AttackManager.AttackTarget(weaponId, other.gameObject, penetrate, (affecter) => affecter.Knockback(gameObject));
            if(penetrate == stats.Penetrate)
            {
                Destroy(gameObject);
            }
        }
    }
}
