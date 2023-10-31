using UnityEngine;

public class Diagum : BaseWeapon
{
    private void Awake()
    {
        penetrate = 0;
        stats = WeaponBundle.GetWeapon("Diagum").stats;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy") && penetrate < stats.Penetrate)
        {
            AttackManager.AttackTarget("Diagum", other.gameObject, penetrate, (affecter) => affecter.Knockback(gameObject));
            penetrate++;
            if(penetrate == stats.Penetrate)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
