using System.Collections;
using UnityEngine;

public class Affecter : MonoBehaviour
{
    public Status status {
        get
        {
            return _status;
        }
        private set
        {
            lastStatus = _status;
            if(value == Status.Idle) _status = Status.Idle;
            else
            {
                if(_status == Status.Idle)
                    _status = value;
                else
                    _status = Status.Multiple;
            }
        }
    }
    private Animator animator;
    private Rigidbody2D rigid;
    private Status _status = Status.Idle;
    private Status lastStatus;
    private readonly int forcePower = 10;
    public enum Status
    {
        Idle,
        Knockback,
        Sturn,
        Multiple
    }

    public void Reset()
    {
        status = Status.Idle;
        _status = Status.Idle;
        StopAllCoroutines();
    }

    public void AttackAnimate()
    {
        animator.SetTrigger("Attack");
    }

    public void Knockback(GameObject target)
    {
        if(gameObject.CompareTag("Player")) return; // 플레이어는 밀릴 수 없음
        status = Status.Knockback;
        Vector2 direction = (transform.position - target.transform.position).normalized;
        rigid.AddForce(direction * forcePower, ForceMode2D.Impulse);
        StartCoroutine(KnockbackReset());
    }

    public IEnumerator ThreeComboKnockback(GameObject source)
    {
        status = Status.Knockback;
        Vector2 direction = (transform.position - source.transform.position).normalized;
        StartCoroutine(ComboKnockback(direction, 0, source));
        yield return new WaitForSeconds(0.2f); // 공격 딜레이
        StartCoroutine(ComboKnockback(direction, 1, source));
        yield return new WaitForSeconds(0.2f); // 공격 딜레이
        StartCoroutine(ComboKnockback(direction, 2, source));
        yield return new WaitForSeconds(0.2f); // 공격 딜레이
        CheckCurrentStatus(Status.Knockback);
    }

    public void Sturn()
    {
        if(gameObject.CompareTag("Player")) return; // 플레이어는 기절할 수 없음
        status = Status.Sturn;
        animator.SetBool("isWalk", false);
        SetColor(Color.yellow);
        StartCoroutine(SturnReset());
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
    }

    private IEnumerator ComboKnockback(Vector2 direction, int i, GameObject source)
    {
        rigid.AddForce(direction * (forcePower - i * 4), ForceMode2D.Impulse);
        AttackManager.AttackTarget("Cex", gameObject, i, source:source);
        yield return new WaitForSeconds(0.2f);
        rigid.velocity = Vector2.zero;
    }

    private IEnumerator KnockbackReset()
    {
        yield return new WaitForSeconds(0.2f);
        rigid.velocity = Vector2.zero;
        CheckCurrentStatus(Status.Knockback);
    }

    private IEnumerator SturnReset()
    {
        yield return new WaitForSeconds(3f);
        SetColor(Color.white);
        CheckCurrentStatus(Status.Sturn);
    }

    private void SetColor(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
    }

    private void CheckCurrentStatus(Status checkStatus)
    {
        if(status == Status.Multiple)
        {
            status = lastStatus;
            lastStatus = Status.Idle;
        }
        else if(status == checkStatus)
        {
            status = Status.Idle;
        }
    }
}
