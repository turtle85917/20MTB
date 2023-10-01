using System;
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

    private void Awake()
    {
        animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        animator.runtimeAnimatorController = enemy.controller;
    }

    private void Update()
    {
        LookAtPlayer();
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
}
