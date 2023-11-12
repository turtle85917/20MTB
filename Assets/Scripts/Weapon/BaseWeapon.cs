using System.Collections;
using _20MTB.Stats;
using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    public WeaponStats stats {protected get; set;}
    public GameObject weaponUser {protected get; set;}
    public WeaponUser weaponUserType {protected get; set;}
    protected int penetrate // 관통 수
    {
        get
        {
            return _penetrate;
        }
        set
        {
            _penetrate = value;
            if(value > 0) count++;
            if(weaponUser?.name == "Jinhe")
            {
                if(_penetrate == stats.Penetrate + stats.Count)
                {
                    weaponUser.GetComponent<Jinhe>().OnBrokenWeapon();
                }
            }
            else
            {
                if(_penetrate == stats.Penetrate && weaponStatus == WeaponStatus.Idle)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
    protected int count     // 무기 사용 횟수 (적한테만)
    {
        get
        {
            return _count;
        }
        set
        {
            if((weaponUser && weaponUser.CompareTag("Enemy")) || weaponUserType == WeaponUser.Enemy)
            {
                _count = value;
                if(_count == stats.Count)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
    protected WeaponStatus weaponStatus;
    protected Rigidbody2D rigid;
    protected new Animation animation;
    protected SpriteRenderer sprite;
    private int _penetrate;
    private int _count;

    public void Init()
    {
        count = 0;
        penetrate = 0;
        _count = 0;
        _penetrate = 0;
        weaponStatus = WeaponStatus.Idle;
        StartCoroutine(FinisingLifeTime());
    }

    public void OnHideAnimEnd()
    {
        gameObject.SetActive(false);
        StealEnemyWeapon();
    }

    private void StealEnemyWeapon()
    {
        if(weaponUser != null && weaponUser.CompareTag("Enemy"))
        {
            EnemyManager.EnemyPool enemyPool = EnemyManager.GetEnemy(weaponUser);
            enemyPool.weapon = null;
        }
    }

    private IEnumerator FinisingLifeTime()
    {
        yield return new WaitForSeconds(stats.Life);
        if(animation != null && animation.GetClip("Hide"))
        {
            animation.Play("Hide");
        }
        else
        {
            gameObject.SetActive(false);
            StealEnemyWeapon();
        }
    }
}
