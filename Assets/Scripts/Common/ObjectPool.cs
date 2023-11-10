using UnityEngine;

public static class ObjectPool
{
    public static GameObject Get(GameObject Parent, string name, GameObject Prefab)
    {
        for(int i = 0; i < Parent.transform.childCount; i++)
        {
            GameObject child = Parent.transform.GetChild(i).gameObject;
            if(!child.activeSelf && child.name == name)
            {
                child.SetActive(true);
                return child;
            }
        }
        GameObject obj = Object.Instantiate(Prefab, Parent.transform, false);
        obj.name = name;
        return obj;
    }
}
