using System.Collections;
using _20MTB.Utillity;
using UnityEngine;

public class Ladle : BaseWeapon
{
    private GameObject target;
    private float combo;

    public new void Init()
    {
        base.Init();
        transform.localPosition = weaponUser.transform.position + Vector3.forward * 1.2f;
        transform.localScale = Vector2.zero;
        rigid.velocity = Vector2.zero;
        animation.Play("Show");
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animation = GetComponent<Animation>();
    }

    private void Update()
    {
        if(weaponStatus == WeaponStatus.Idle)
        {
            if(target == null)
            {
                Scanner.Scan(weaponUser.transform.position, stats.Range, GameUtils.GetTargetTag(weaponUserType), out target);
                if(target == null)
                    animation.Play("Hide");
                else
                {
                    combo = ((Vector3)Game.maxPosition + Camera.main.transform.position - target.transform.position).normalized.y;
                    StartCoroutine(Repair());
                }
                count++;
            }
            else
            {
                rigid.MovePosition(Vector3.MoveTowards(rigid.position, target.transform.position, 40 * Time.deltaTime));
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.Equals(target))
        {
            AttackManager.AttackTarget("Ladle", other.gameObject, Mathf.FloorToInt(-penetrate * combo), (affecter) => affecter.Knockback(gameObject), weaponUser);
            TextManager.WriteComboText(target, Mathf.RoundToInt(combo * 1000));
            StartCoroutine(LadleGGang());
            target = null;
            penetrate++;
        }
    }

    private IEnumerator Repair()
    {
        yield return new WaitForSeconds(0.3f);
        if(weaponUser.CompareTag("Enemy"))
        {
            animation.Play("Hide");
        }
        else
        {
            target = null;
            animation.Play("Hide");
        }
    }

    private IEnumerator LadleGGang()
    {
        rigid.AddForce(target.transform.up * 20);
        rigid.gravityScale = 5;
        weaponStatus = WeaponStatus.GoAway;
        yield return new WaitForSeconds(0.4f);
        weaponStatus = WeaponStatus.Idle;
        rigid.gravityScale = 0;
    }
}
