using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    public SpriteRenderer headSprite;
    public SpriteRenderer bodySprite;
    protected Rigidbody2D rigid;
    protected Animator animator;

    private void Awake()
    {
        Init();
    }

    protected abstract void Init();
}
