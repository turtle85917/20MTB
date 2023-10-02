using System;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public static FollowCamera instance {get; private set;}
    [SerializeField] private Vector2 maxPoint;
    [SerializeField] private Vector2 minPoint;

    public Vector3 MovePosition(Vector2 position, float z)
    {
        return new(Math.Max(minPoint.x, Math.Min(position.x, maxPoint.x)), Math.Max(minPoint.y, Math.Min(position.y, maxPoint.y)), z);
    }

    private void Awake()
    {
        instance = this;
    }

    private void LateUpdate()
    {
        transform.position = MovePosition(Player.instance.transform.position, transform.position.z);
    }
}
