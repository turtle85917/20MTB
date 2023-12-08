using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    protected Rigidbody2D rigid;
    protected Animator animator;

    private void Start()
    {
        Init();
    }

    protected abstract void Init();

    public abstract void OnDie();
}
