using System;
using System.Collections.Generic;
using UnityEngine;

public static class Scanner
{
    public static List<GameObject> ScanAll(Vector2 origin, float radius, string tag)
    {
        List<GameObject> result = new List<GameObject>(){};
        ProcessRaycast(origin, radius, (RaycastHit2D raycast) => {
            if(raycast.collider.CompareTag(tag))
            {
                result.Add(raycast.collider.gameObject);
            }
        });
        return result;
    }

    public static GameObject Scan(Vector2 origin, float radius, string tag)
    {
        float r = radius;
        GameObject result = null;
        ProcessRaycast(origin, radius, (RaycastHit2D raycast) => {
            if(raycast.collider.CompareTag(tag))
            {
                float distance = Vector3.Distance(origin, raycast.transform.position);
                if(distance < r)
                {
                    r = distance;
                    result = raycast.collider.gameObject;
                }
            }
        });
        return result;
    }

    public static void Scan(Vector2 origin, float radius, string tag, out GameObject target)
    {
        float r = radius;
        GameObject result = null;
        ProcessRaycast(origin, radius, (RaycastHit2D raycast) => {
            if(raycast.collider.CompareTag(tag))
            {
                float distance = Vector3.Distance(origin, raycast.transform.position);
                if(distance < r)
                {
                    r = distance;
                    result = raycast.collider.gameObject;
                }
            }
        });
        target = result;
    }

    private static void ProcessRaycast(Vector2 origin, float radius, Action<RaycastHit2D> processFunc)
    {
        RaycastHit2D[] raycasts = Physics2D.CircleCastAll(origin, radius, Vector2.right);
        foreach(RaycastHit2D raycast in raycasts)
        {
            processFunc(raycast);
        }
    }
}
