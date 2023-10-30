using System.Collections;
using _20MTB.Stats;
using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    public string weaponId;
    public int penetrate {protected get; set;}
    public WeaponStats stats {protected get; set;}
    public GameObject weaponUser {protected get; set;}
    public WeaponUser weaponUserType {protected get; set;}
    protected WeaponStatus weaponStatus;
    protected Rigidbody2D rigid;
    protected new Animation animation;
    protected SpriteRenderer sprite;

    public void Init()
    {
        penetrate = 0;
        weaponStatus = WeaponStatus.Idle;
        StartCoroutine(FinisingLifeTime());
    }

    public void OnHideAnimEnd()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator FinisingLifeTime()
    {
        yield return new WaitForSeconds(stats.Life);
        if(animation != null)
        {
            animation.Play("Hide");
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
