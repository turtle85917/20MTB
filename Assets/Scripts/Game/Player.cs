using UnityEngine;

public class Player : Affecter
{
    public Vector2 Movement
    {
        get
        {
            if(movement == Vector2.zero)
            {
                return lastMovement;
            }
            return movement;
        }
    }
    public static Player instance {get; private set;}
    [Header("캐릭터 파츠")]
    [SerializeField] private SpriteRenderer headSprite;
    [SerializeField] private SpriteRenderer bodySprite;
    private Vector2 movement;
    private Vector2 lastMovement;

    public void Attack()
    {
        animator.SetTrigger("Attack");
    }

    private void Awake()
    {
        instance = this;
        movement = Vector2.zero;
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        animator.runtimeAnimatorController = Game.playerData.data.controller;
    }

    private void Update()
    {
        movement = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );
        if(movement != Vector2.zero)
            lastMovement = movement;
        if(movement != Vector2.zero)
        {
            animator.SetBool("isWalk", true);
            rigidbody.MovePosition(Game.MovePositionLimited(rigidbody.position + movement * Game.playerData.data.stats.MoveSpeed * Time.deltaTime, transform.position.z));
            rigidbody.velocity = Vector2.zero;
            SetFlipX();
        }
        else
        {
            animator.SetBool("isWalk", false);
        }
    }

    private void SetFlipX()
    {
        headSprite.flipX = movement.x < 0;
        bodySprite.flipX = movement.x < 0;
    }
}
