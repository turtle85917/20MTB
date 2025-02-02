using System.Collections;
using _20MTB.Stats;
using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    public WeaponStats stats {protected get; set;}
    public GameObject weaponUser {protected get; set;}
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
                if(_penetrate == stats.Penetrate)
                {
                    weaponUser.GetComponent<Jinhe>().OnDie();
                }
            }
            else
            {
                if(!isHide_MaxPentrate && _penetrate == stats.Penetrate && weaponStatus == WeaponStatus.Idle)
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
            if(weaponUser && weaponUser.CompareTag("Enemy"))
            {
                _count = value;
                if(_count == stats.Penetrate) gameObject.SetActive(false);
            }
        }
    }

    protected WeaponStatus weaponStatus;
    protected Rigidbody2D rigid;
    protected new Animation animation;
    protected SpriteRenderer sprite;

    protected bool isHide_MaxPentrate;

    private int _penetrate;
    private int _count;

    public void Init()
    {
        _count = 0;
        _penetrate = 0;
        count = 0;
        penetrate = 0;
        weaponStatus = WeaponStatus.Idle;
        StartCoroutine(FinisingLifeTime());
    }

    public void OnHideAnimEnd()
    {
        gameObject.SetActive(false);
        StealEnemyWeapon();
    }

    protected void StealEnemyWeapon()
    {
        if(weaponUser != null && weaponUser.CompareTag("Enemy"))
        {
            EnemyPool enemyPool = EnemyManager.GetEnemy(weaponUser);
            if(enemyPool == null) return;
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
