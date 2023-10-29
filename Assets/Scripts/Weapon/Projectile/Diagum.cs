using UnityEngine;

public class Diagum : BaseWeapon
{
    private void Awake()
    {
        penetrate = 0;
        stats = WeaponBundle.GetWeapon(weaponId).stats;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy") && penetrate < stats.Penetrate)
        {
            AttackManager.AttackTarget(weaponId, other.gameObject, penetrate, (affecter) => affecter.Knockback(gameObject));
            penetrate++;
            if(penetrate == stats.Penetrate)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
