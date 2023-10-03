using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultWeapon : MonoBehaviour
{
    [SerializeField] private GameObject Blow;
    [SerializeField] private GameObject Star;
    private Weapon weapon;

    private void Start()
    {
        weapon = WeaponBundle.GetWeapon(Game.instance.playerData.defaultWeapon);
        switch(weapon.weapon.WeaponId)
        {
            case "Wakchori":
                StartCoroutine(Wakchori());
                break;
            case "MagicWand":
                StartCoroutine(MagicWand());
                break;
        }
    }

    private IEnumerator Wakchori()
    {
        WaitForSeconds wait = new(weapon.stats.Cooldown);
        while(true)
        {
            yield return wait;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 distance = Player.instance.transform.position - (Vector3)mousePosition;
            GameObject blow = ObjectPool.Get(
                Game.instance.PoolManager,
                "Blow",
                () => Instantiate(Blow, Game.instance.PoolManager.transform, false)
            );
            blow.name = "Blow";
            blow.transform.rotation = LookAtMouse();
            Blow script = blow.GetComponent<Blow>();
            script.Reset(weapon.stats, distance.normalized * -1);
            yield return new WaitForSeconds(weapon.stats.Life);
            blow.SetActive(false);
        }
    }

    private IEnumerator MagicWand()
    {
        List<GameObject> targets = new(){};
        WaitForSeconds wait = new(weapon.stats.Cooldown);
        while(true)
        {
            yield return wait;
            for(int i = 0; i < weapon.stats.ProjectileCount; i++)
            {
                GameObject star = ObjectPool.Get(
                    Game.instance.PoolManager,
                    "Star",
                    () => Instantiate(Star, Game.instance.PoolManager.transform, false)
                );
                star.name = "Star" + i;
                star.transform.localPosition = Player.instance.transform.position;
                star.GetComponent<Star>().Reset(weapon.stats, targets);
            }
        }
    }

    private Quaternion LookAtMouse()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 distance = transform.position.x < mousePosition.x
            ? (Vector3)mousePosition - transform.position
            : transform.position - (Vector3)mousePosition
        ;
        return Quaternion.AngleAxis((float)(Math.Atan2(distance.y, distance.x) * Mathf.Rad2Deg), Vector3.forward);
    }
}
