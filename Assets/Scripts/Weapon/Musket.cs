using _20MTB.Utillity;
using UnityEngine;

public class Musket : BaseWeapon
{
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        GameObject target = Scanner.Scan(Game.Player.transform.position, stats.Range, weaponUserType == WeaponUser.Player ? "Enemy" : "Player");
        if(target != null)
        {
            sprite.flipX = false;
            transform.rotation = GameUtils.LookAtTarget(transform.position, target.transform.position);
        }
        else
        {
            sprite.flipX = GameUtils.GetDirectionFromTarget(weaponUser) == -1;
            transform.rotation = Quaternion.identity;
        }
    }
}
