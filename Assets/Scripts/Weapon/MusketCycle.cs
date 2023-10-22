using System.Collections;
using _20MTB.Stats;
using UnityEngine;

public class MusketCycle : MonoBehaviour, IExecuteWeapon
{
    private WeaponStats stats;
    private GameObject Bullet;

    public void ExecuteWeapon(GameObject weaponUserVal)
    {
        Weapon weapon = WeaponBundle.GetWeapon("Musket");
        stats = weapon.stats;
        Bullet = (GameObject)weapon.weapon.resources[1];
        GameObject musket = Instantiate((GameObject)weapon.weapon.resources[0], Game.instance.PlayerWeapons.transform, false);
        musket.name = "머스켓";
        musket.transform.localPosition = Vector3.right;
        StartCoroutine(WeaponCycle(musket));
    }

    private IEnumerator WeaponCycle(GameObject Musket)
    {
        while(true)
        {
            yield return new WaitForSeconds(stats.Cooldown);
            GameObject bullet = ObjectPool.Get(
                Game.instance.PoolManager,
                "Bullet",
                (parent) => Instantiate(Bullet, parent.transform, false)
            );
            bullet.transform.position = Musket.transform.position;
            Bullet script = bullet.GetComponent<Bullet>();
            script.stats = stats;
            script.Reset(Musket);
            yield return new WaitForSeconds(stats.Life);
            bullet.SetActive(false);
        }
    }
}
