using UnityEngine;

public static class Scanner
{
    public static GameObject Scan(Vector2 origin, float radius, string tag)
    {
        float r = radius;
        GameObject result = null;
        RaycastHit2D[] raycasts = Physics2D.CircleCastAll(origin, r, Vector2.right);
        foreach(RaycastHit2D raycast in raycasts)
        {
            if(raycast.collider.CompareTag(tag))
            {
                float distance = Vector3.Distance(origin, raycast.transform.position);
                if(distance < r)
                {
                    r = distance;
                    result = raycast.collider.gameObject;
                }
            }
        }
        return result;
    }
}
