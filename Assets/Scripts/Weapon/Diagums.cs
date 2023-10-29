using UnityEngine;

public class Diagums : BaseWeapon
{
    [SerializeField] private GameObject Diagum;

    public new void Init()
    {
        base.Init();
        transform.position = Game.PlayerObject.transform.position;
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
        transform.position = Game.PlayerObject.transform.position;
        transform.Rotate(0, 0, stats.ProjectileSpeed, Space.Self);
    }
}
