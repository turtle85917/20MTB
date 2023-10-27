using System;
using System.Collections;
using UnityEngine;

public class Affecter : MonoBehaviour
{
    protected Status status;
    protected Animator animator;
    protected new Rigidbody2D rigidbody;
    private readonly int forcePower = 10;
    public enum Status
    {
        Idle,
        Knockback,
        Sturn
    }

    public void AttackAnimate()
    {
        animator.SetTrigger("Attack");
    }

    public void Knockback(GameObject target)
    {
        status = Status.Knockback;
        Vector2 direction = (transform.position - target.transform.position).normalized;
        rigidbody.AddForce(direction * forcePower, ForceMode2D.Impulse);
        StartCoroutine(Reset());
    }

    public void Sturn(Action callbackFunc = null)
    {
        SetColor(Color.yellow);
        status = Status.Sturn;
        animator.SetBool("isWalk", false);
        StartCoroutine(Reset(3f, callbackFunc:callbackFunc));
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private IEnumerator Reset(float duration = 0.2f, Action callbackFunc = null)
    {
        yield return new WaitForSeconds(duration);
        status = Status.Idle;
        rigidbody.velocity = Vector2.zero;
        SetColor(Color.white);
        if(callbackFunc != null)
        {
            callbackFunc();
        }
    }

    private void SetColor(Color color)
    {
        BaseController baseController =  transform.GetComponent<BaseController>();
        baseController.headSprite.color = color;
        baseController.bodySprite.color = color;
    }
}
