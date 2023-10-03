using System.Collections.Generic;
using UnityEngine;

public static class Scanner
{
    public static GameObject ScanFilter(Vector2 origin, float radius, string tag, List<GameObject> list)
    {
        float r = radius;
        GameObject result = null;
        RaycastHit2D[] raycasts = Physics2D.CircleCastAll(origin, r, Vector2.right);
        foreach(RaycastHit2D raycast in raycasts)
        {
            if(raycast.collider.CompareTag(tag) && !list.Contains(raycast.collider.gameObject))
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

    public static List<GameObject> ScanAll(Vector2 origin, float radius, string tag)
    {
        List<GameObject> result = new(){};
        RaycastHit2D[] raycasts = Physics2D.CircleCastAll(origin, radius, Vector2.right);
        foreach(RaycastHit2D raycast in raycasts)
        {
            if(raycast.collider.CompareTag(tag))
            {
                result.Add(raycast.collider.gameObject);
            }
        }
        return result;
    }

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
