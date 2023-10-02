using UnityEngine;

public static class Scanner
{
    public static GameObject Scan(Vector2 origin, float radius, string tag = null)
    {
        float r = radius;
        GameObject result = Physics2D.CircleCast(origin, r, Vector2.right).collider.gameObject;
        while(result != null)
        {
            if(tag != null && result.CompareTag(tag))
            {
                float distance = Vector3.Distance(origin, result.transform.position);
                if(distance < r)
                {
                    r = distance;
                    result = Physics2D.CircleCast(origin, r, Vector2.right).collider.gameObject;
                }
            }
        }
        return result;
    }
}
