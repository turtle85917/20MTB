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
                ChangeZ(gameObject, 1);
                ChangeZ(other.gameObject, 0);
            }else
            {
                ChangeZ(gameObject, 0);
                ChangeZ(other.gameObject, 1);
            }
        }
    }

    private void ChangeZ(GameObject target, float z)
    {
        target.transform.position = new(target.transform.position.x, target.transform.position.y, z);
    }
}
