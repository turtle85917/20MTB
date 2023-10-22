using _20MTB.Stats;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public WeaponStats stats {private get; set;}
    private Vector3 direction;
    private new Rigidbody2D rigidbody;

    public void Reset(GameObject target)
    {
        direction = target.transform.right;
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rigidbody.AddForce(new Vector2(direction.x * 50, 0));
    }
}
