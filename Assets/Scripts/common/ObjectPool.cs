using System;
using UnityEngine;

public static class ObjectPool
{
    public static GameObject Get(GameObject Parent, string name, Func<GameObject> CreateFunc)
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
        return CreateFunc();
    }

    public static GameObject SpawnExp(Transform target)
    {
        GameObject exp = UnityEngine.Object.Instantiate(Game.instance.Exp, Game.instance.PoolManager.transform, false);
        exp.name = "Exp";
        exp.transform.position = target.position;
        return exp;
    }
}
