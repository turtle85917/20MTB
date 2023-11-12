using UnityEngine;

public class SpinManager : BaseWeapon
{
    public Weapon weapon;

    public new void Init()
    {
        base.Init();
        transform.position = weaponUser.transform.position;
        transform.rotation = Quaternion.identity;
        for(int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            Destroy(child);
        }
        for(int i = 0; i < stats.ProjectileCount; i++)
        {
            GameObject revert = Instantiate((GameObject)weapon.weapon.resources[1], transform, false);
            revert.name = "Revert";
            revert.transform.localRotation = Quaternion.Euler(0, 0, 360 / stats.ProjectileCount * i);
            revert.transform.localPosition = revert.transform.up * 1.24f;
            Revert script = revert.GetComponent<Revert>();
            script.stats = weapon.stats;
            script.weaponUser = weaponUser;
            script.Init();
        }
    }

    private void Update()
    {
        transform.position = weaponUser.transform.position;
        transform.Rotate(0, 0, stats.ProjectileSpeed, Space.Self);
    }
}
