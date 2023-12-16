using System.Collections;
using _20MTB.Utillity;
using UnityEngine;

public class Jinhe : MonoBehaviour
{
    public float life {private get; set;}
    public string targetTag {private get; set;}
    public EnemyPool weaponOwner;
    private GameObject followTarget;
    private Rigidbody2D rigid;
    private Animator animator;
    private SpriteRenderer sprite;
    private bool isDie;

    public void Init()
    {
        isDie = false;
        rigid.velocity = Vector2.zero;
        StartCoroutine(FinishingWeaponLifetime());
    }

    public void OnDie()
    {
        isDie = true;
        animator.SetTrigger("isDied");
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(isDie) return;
        if(followTarget == null)
        {
            GameObject[] targets = Scanner.ScanAll(transform.position, 80f, targetTag);
            followTarget = targets.Length == 0 ? null : targets[Random.Range(0, targets.Length)];
        }
        else if(Vector3.Distance(followTarget.transform.position, Player.@object.transform.position) > 10f)
        {
            followTarget = null;
        }
    }

    private void FixedUpdate()
    {
        if(isDie) return;
        if(followTarget == null)
        {
            animator.SetBool("isWalk", false);
        }
        else
        {
            animator.SetBool("isWalk", true);
            sprite.flipX = transform.position.x < followTarget.transform.position.x;
            Vector3 position = Vector3.MoveTowards(rigid.position, followTarget.transform.position, 1.4f * Time.fixedDeltaTime);
            rigid.MovePosition(GameUtils.MovePositionLimited(position));
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.Equals(followTarget))
        {
            followTarget = null;
        }
    }

    private void OnDieAnimEnd()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }

    private IEnumerator FinishingWeaponLifetime()
    {
        yield return new WaitForSeconds(life);
        OnDie();
    }
}
