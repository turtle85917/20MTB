using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyData enemy;
    private Animator animator;
    private Rigidbody2D Rigidbody;
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
    {}
}
