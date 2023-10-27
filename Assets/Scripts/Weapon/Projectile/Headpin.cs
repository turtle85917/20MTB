using UnityEngine;

public class Headpin : BaseWeapon
{
    public new void Init()
    {
        base.Init();
        transform.rotation = Quaternion.AngleAxis(Random.Range(-360f, 360f), Vector3.forward);
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rigid.AddForce(transform.right * 22);
    }

    private void OnBecameInvisible()
    {
        if(transform.rotation.z < 0.5f)
        {
            transform.rotation = transform.rotation * Quaternion.AngleAxis(Random.Range(45f, 90f), Vector3.forward);
        }
        else
        {
            transform.rotation = transform.rotation * Quaternion.AngleAxis(Random.Range(-45f, -90f), Vector3.forward);
        }
    }
}
