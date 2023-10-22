using System.Collections;
using _20MTB.Stats;
using _20MTB.Utillity;
using UnityEngine;

public class MusketCycle : MonoBehaviour, IExecuteWeapon
{
    private WeaponStats stats;
    private GameObject weaponUser;
    private GameObject Bullet;
    private GameObject Musket;
    private SpriteRenderer MusketSpriteRenderer;

    public void ExecuteWeapon(GameObject weaponUserVal)
    {
        Weapon weapon = WeaponBundle.GetWeapon("Musket");
        stats = weapon.stats;
        weaponUser = weaponUserVal;
        Bullet = (GameObject)weapon.weapon.resources[1];
        GameObject musket = Instantiate((GameObject)weapon.weapon.resources[0], Game.instance.PlayerWeapons.transform, false);
        musket.name = "머스켓";
        musket.transform.localPosition = Vector3.right;
        MusketSpriteRenderer = musket.GetComponent<SpriteRenderer>();
        Musket = musket;
        StartCoroutine(WeaponCycle());
    }

    private void Update()
    {
        Musket.transform.localPosition = Vector3.right * GameUtils.GetDirectionFromTarget(weaponUser);
        MusketSpriteRenderer.flipX = Musket.transform.localPosition.x < 0;
    }

    private IEnumerator WeaponCycle()
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
            GameObject enemy = Scanner.Scan(transform.position, stats.Range, "Enemy");
            Bullet script = bullet.GetComponent<Bullet>();
            script.stats = stats;
            script.Reset(enemy);
            yield return new WaitForSeconds(stats.Life);
            bullet.SetActive(false);
        }
    }
}
