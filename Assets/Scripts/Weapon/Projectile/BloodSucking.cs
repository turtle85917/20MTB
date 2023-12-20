using System.Collections;
using _20MTB.Utillity;
using UnityEngine;

public class BloodSucking : BaseWeapon
{
    private ParticleSystem particle;

    public new void Init()
    {
        base.Init();
        particle.Stop();
        if(weaponUser.CompareTag("Player"))
        {
            transform.position = GetMaxPosition(-1);
            StartCoroutine(AttackEnemy());
        }
        else
        {
            transform.position = weaponUser.transform.position;
            StartCoroutine(AttackPlayer());
        }
    }

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    private IEnumerator AttackPlayer()
    {
        particle.Play();
        yield return new WaitForSeconds(0.3f);
        transform.position = Player.@object.transform.position;
        AttackManager.AttackTarget("BloodSucking", Player.@object, 0, source:weaponUser);
        AttackManager.AttackTarget(-10, weaponUser, EnemyManager.GetEnemy(weaponUser));
    }

    private IEnumerator AttackEnemy()
    {
        particle.Play();
        yield return new WaitForSeconds(0.3f);
        transform.position = GetMaxPosition(1);
        RaycastHit2D[] raycastHits = Physics2D.LinecastAll(GetMaxPosition(-1), GetMaxPosition(1));
        int i = 0;
        foreach(RaycastHit2D raycast in raycastHits)
        {
            if(raycast.collider.gameObject.CompareTag(GameUtils.GetTargetTag(weaponUser)))
            {
                if(++i == stats.Penetrate) break;
                AttackManager.AttackTarget("BloodSucking", raycast.collider.gameObject, 0, source:weaponUser);
                if(i < stats.Penetrate / 2)
                    AttackManager.AttackTarget(-i, weaponUser, null);
            }
        }
    }

    private Vector3 GetMaxPosition(int direction = 1)
    {
        return new Vector3(Game.maxPosition.x * direction + Camera.main.transform.position.x, weaponUser.transform.position.y);
    }
}
