using System;
using System.Collections;
using UnityEngine;

public class Affecter : MonoBehaviour
{
    protected Status status;
    protected new Rigidbody2D rigidbody;
    private readonly int forcePower = 20;
    private readonly WaitForSeconds wait = new WaitForSeconds(0.2f);
    public enum Status
    {
        Idle,
        Knockback,
        Sturn,
        Die
    }

    public void Knockback(GameObject target)
    {
        status = Status.Knockback;
        Vector2 direction = (transform.position - target.transform.position).normalized;
        rigidbody.AddForce(direction * forcePower, ForceMode2D.Impulse);
        StartCoroutine(Reset());
    }

    public virtual void Sturn(Action callbackFunc = null)
    {
        if(status != Status.Idle) return;
        status = Status.Sturn;
        StartCoroutine(Reset(callbackFunc));
    }

    private IEnumerator Reset(Action callbackFunc = null)
    {
        yield return wait;
        status = Status.Idle;
        rigidbody.velocity = Vector2.zero;
        if(callbackFunc != null)
        {
            callbackFunc();
        }
    }
}
