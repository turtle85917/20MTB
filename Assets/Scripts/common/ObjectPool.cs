using System;
using UnityEngine;

public static class ObjectPool
{
    public static GameObject Get(GameObject Parent, Func<GameObject> CreateFunc, Action<GameObject> ProcessChild = null)
    {
        for(int i = 0; i < Parent.transform.childCount; i++)
        {
            GameObject child = Parent.transform.GetChild(i).gameObject;
            if(!child.activeSelf)
            {
                child.SetActive(true);
                if(ProcessChild != null)
                    ProcessChild(child);
                return child;
            }
        }
        return CreateFunc();
    }
}
