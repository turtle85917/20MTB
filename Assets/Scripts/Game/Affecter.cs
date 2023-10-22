using System;
using System.Collections;
using UnityEngine;

public class Affecter : MonoBehaviour
{
    protected Status status;
    protected Animator animator;
    protected new Rigidbody2D rigidbody;
    private readonly int forcePower = 20;
    public enum Status
    {
        Idle,
        Knockback,
        Sturn,
        Die
    }

    public void Knockback(GameObject target)
    {
        if(status != Status.Die)
            status = Status.Knockback;
        Vector2 direction = (transform.position - target.transform.position).normalized;
        rigidbody.AddForce(direction * forcePower, ForceMode2D.Impulse);
        StartCoroutine(Reset());
    }

    public virtual void Sturn(Action callbackFunc = null)
    {
        if(status != Status.Die)
            status = Status.Sturn;
        animator.SetBool("isWalk", false);
        StartCoroutine(Reset(3f, callbackFunc:callbackFunc));
    }

    private IEnumerator Reset(float duration = 0.2f, Action callbackFunc = null)
    {
        yield return new WaitForSeconds(duration);
        if(status != Status.Die)
            status = Status.Idle;
        rigidbody.velocity = Vector2.zero;
        if(callbackFunc != null)
        {
            callbackFunc();
        }
    }
}
