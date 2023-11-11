using System.Collections;
using _20MTB.Utillity;
using UnityEngine;

public class Rock : BaseWeapon
{
    public int index;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag(GameUtils.GetTargetTag(weaponUser)) && index < stats.Penetrate)
        {
            weaponStatus = WeaponStatus.GoAway;
            AttackManager.AttackTarget("Sledgehammer", other.gameObject, index, source:weaponUser);
        }
    }

    private void OnEnable()
    {
        StartCoroutine(BrokenRock());
    }

    private IEnumerator BrokenRock()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
