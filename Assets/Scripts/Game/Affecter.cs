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

    public void AttackAnimate()
    {
        animator.SetTrigger("Attack");
    }

    public void Knockback(GameObject target)
    {
        status = Status.Knockback;
        Vector2 direction = (transform.position - target.transform.position).normalized;
        rigid.AddForce(direction * forcePower, ForceMode2D.Impulse);
        StartCoroutine(KnockbackReset());
    }

    public void Sturn()
    {
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
        BaseController baseController =  transform.GetComponent<BaseController>();
        baseController.headSprite.color = color;
        baseController.bodySprite.color = color;
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
