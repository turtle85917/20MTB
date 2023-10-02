using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyData enemy;
    private Animator animator;
    private Rigidbody2D Rigidbody;
    [SerializeField] private GameObject Head;
    [Header("캐릭터 파츠")]
    [SerializeField] private SpriteRenderer headSprite;
    [SerializeField] private SpriteRenderer bodySprite;
    private bool knockbacking;
    private int forceAdd = 20;

    public void Knockback(GameObject target)
    {
        knockbacking = true;
        animator.SetBool("isWalk", false);
        animator.SetBool("isKnockback", true);
        Rigidbody.velocity = Vector2.zero;
        StartCoroutine(Knockbacking(target));
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        LookAtPlayer();
        bodySprite.flipX = transform.position.x > Player.instance.transform.position.x;
        animator.SetBool("isWalk", true);
        if(!knockbacking)
            Rigidbody.MovePosition(Vector3.MoveTowards(Rigidbody.position, Player.instance.transform.position, enemy.stats.MoveSpeed * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Player.health -= 2;
            Player.instance.Knockback(gameObject);
            Damage.instance.WriteDamage(other.gameObject, 2);
        }
    }

    private void LookAtPlayer()
    {
        Vector3 playerPosition = Player.instance.transform.position;
        Vector2 distance = transform.position.x < playerPosition.x
            ? playerPosition - transform.position
            : transform.position - playerPosition
        ;
        float angle = (float)(Math.Atan2(distance.y, distance.x) * 180 / Math.PI);
        headSprite.flipX = transform.position.x > playerPosition.x;
        if(Math.Abs(angle) < 80)
            Head.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        else
            Head.transform.rotation = Quaternion.identity;
    }

    IEnumerator Knockbacking(GameObject target)
    {
        Vector2 direction = (transform.position - target.transform.position).normalized;
        Rigidbody.AddForce(direction * forceAdd, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.2f);
        knockbacking = false;
        Rigidbody.velocity = Vector3.zero;
        animator.SetBool("isKnockback", false);
    }
}
