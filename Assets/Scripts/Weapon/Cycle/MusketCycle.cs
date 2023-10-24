using System.Collections;
using _20MTB.Stats;
using _20MTB.Utillity;
using UnityEngine;

public class MusketCycle : MonoBehaviour, IExecuteWeapon
{
    private GameObject target;
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
        GameObject musket = Instantiate((GameObject)weapon.weapon.resources[0], Game.PlayerWeapons.transform, false);
        musket.name = "머스켓";
        musket.transform.localPosition = Vector3.right * 0.5f;
        MusketSpriteRenderer = musket.GetComponent<SpriteRenderer>();
        Musket = musket;
        StartCoroutine(WeaponCycle());
    }

    private void Update()
    {
        target = Scanner.Scan(Game.Player.transform.position, stats.Range, "Enemy");
        if(target != null)
        {
            MusketSpriteRenderer.flipX = false;
            Musket.transform.rotation = GameUtils.LookAtTarget(Musket.transform.position, target.transform.position);
        }
        else
        {
            MusketSpriteRenderer.flipX = GameUtils.GetDirectionFromTarget(weaponUser) == -1;
            Musket.transform.rotation = Quaternion.identity;
        }
    }

    private IEnumerator WeaponCycle()
    {
        while(true)
        {
            yield return new WaitForSeconds(stats.Cooldown);
            GameObject bullet = ObjectPool.Get(
                Game.PoolManager,
                "Bullet",
                (parent) => Instantiate(Bullet, parent.transform, false)
            );
            bullet.transform.position = Musket.transform.position;
            // bullet.transform.rotation = MultipleQuaternionInt(Musket.transform.rotation, MusketSpriteRenderer.flipX ? 1 : -1);
            Bullet script = bullet.GetComponent<Bullet>();
            script.stats = stats;
            script.Reset(Musket, target);
            yield return new WaitForSeconds(stats.Life);
            bullet.SetActive(false);
        }
    }

    private Quaternion MultipleQuaternionInt(Quaternion quaternion, int value)
    {
        return new Quaternion(quaternion.x * value, quaternion.y * value, quaternion.z * value, quaternion.w * value);
    }
}
