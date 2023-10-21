using _20MTB.Stats;
using UnityEngine;

public class Diagums : MonoBehaviour
{
    public WeaponStats Stats {
        get {
            return stats;
        }
    }
    [SerializeField] private GameObject Diagum;
    private WeaponStats stats;

    public void Reset(WeaponStats statsVal)
    {
        stats = statsVal;
        transform.rotation = Quaternion.identity;
        for(int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            Destroy(child);
        }
        for(int i = 0; i < stats.ProjectileCount; i++)
        {
            GameObject diagum = Instantiate(Diagum, transform, false);
            diagum.name = "Diagum";
            diagum.transform.localRotation = Quaternion.Euler(0, 0, 360 / stats.ProjectileCount * i);
            diagum.transform.localPosition = diagum.transform.up * 1.2f;
        }
    }

    private void Update()
    {
        transform.Rotate(0, 0, stats.ProjectileSpeed, Space.Self);
    }
}
