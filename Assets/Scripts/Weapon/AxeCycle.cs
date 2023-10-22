using System.Collections;
using _20MTB.Stats;
using UnityEngine;

public class AxeCycle : MonoBehaviour, IExecuteWeapon
{
    private WeaponStats stats;
    private GameObject AxePrefab;

    public void ExecuteWeapon(GameObject weaponUserVal)
    {
        Weapon weapon = WeaponBundle.GetWeapon("Axe");
        stats = weapon.stats;
        AxePrefab = weapon.weapon.resources[0] as GameObject;
        StartCoroutine(WeaponCycle(weaponUserVal));
    }

    private IEnumerator WeaponCycle(GameObject weaponUser)
    {
        while(true)
        {
            yield return new WaitForSeconds(stats.Cooldown);
            GameObject axe = ObjectPool.Get(
                Game.instance.PoolManager,
                "Axe",
                (parent) => Instantiate(AxePrefab, parent.transform, false)
            );
            axe.GetComponent<AxeExecute>().Reset(weaponUser);
            yield return new WaitForSeconds(stats.Life);
            axe.SetActive(false);
        }
    }
}
