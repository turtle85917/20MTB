using UnityEngine;

public class Bullet : BaseWeapon
{
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy") && gameObject.activeSelf)
        {
            AttackManager.AttackTarget(weaponId, other.gameObject, penetrate);
            penetrate++;
            if(penetrate == stats.Penetrate)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
