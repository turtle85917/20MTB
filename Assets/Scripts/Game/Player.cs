using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 movement;
    public static Player instance {get; private set;}
    private Animator animator;
    private Rigidbody2D Rigidbody;
    [Header("캐릭터 파츠")]
    [SerializeField] private SpriteRenderer headSprite;
    [SerializeField] private SpriteRenderer bodySprite;

    private void Awake()
    {
        instance = this;
        movement = Vector2.zero;
        animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        animator.runtimeAnimatorController = Game.instance.playerData.controller;
    }

    private void Update()
    {
        movement = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );
        if(movement != Vector2.zero)
        {
            animator.SetBool("isWalk", true);
            Rigidbody.MovePosition(Camera.instance.MovePosition(Rigidbody.position + movement * Game.instance.playerData.stats.MoveSpeed * Time.deltaTime, transform.position.z));
            Rigidbody.velocity = Vector2.zero;
            SetFlipX();
        }else
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
