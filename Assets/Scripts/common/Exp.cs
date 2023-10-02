using UnityEngine;

public class Exp : MonoBehaviour
{
    public GameObject target;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(target != null)
        {
            transform.position = target.transform.position;
        }
    }

    private void OnEnable()
    {
        animator.SetTrigger("isAppear");
    }
}
