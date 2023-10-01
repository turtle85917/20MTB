using UnityEngine;

public class CollisionObjectLayer : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayout;

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.IsTouchingLayers(targetLayout))
        {
            if(other.transform.position.y < transform.position.y)
            {
                ChangeZ(other.gameObject, 1, 0);
            }else
            {
                ChangeZ(other.gameObject, 0, 1);
            }
        }
    }

    private void ChangeZ(GameObject target, int z1, int z2)
    {
        transform.position = new(transform.position.x, transform.position.y, z1);
        target.transform.position = new(target.transform.position.x, target.transform.position.y, z2);
    }
}
