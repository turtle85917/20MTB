using System;
using UnityEngine;

public static class ObjectPool
{
    public static GameObject Get(GameObject Parent, string name, Func<GameObject, GameObject> CreateFunc)
    {
        for(int i = 0; i < Parent.transform.childCount; i++)
        {
            GameObject child = Parent.transform.GetChild(i).gameObject;
            if(!child.activeSelf && child.name.StartsWith(name))
            {
                child.SetActive(true);
                return child;
            }
        }
        GameObject obj = CreateFunc(Parent);
        obj.name = name;
        return obj;
    }
}
