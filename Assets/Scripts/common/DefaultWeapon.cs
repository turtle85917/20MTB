using System;
using System.Collections;
using UnityEngine;

public class DefaultWeapon : MonoBehaviour
{
    [SerializeField] private GameObject Blow;
    private Weapon weapon;

    private void Start()
    {
        weapon = WeaponBundle.GetWeapon(Game.instance.playerData.defaultWeapon);
        switch(weapon.weapon.WeaponId)
        {
            case "Wakchori":
                StartCoroutine(Wakchori());
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
                () => Instantiate(Blow, Game.instance.PoolManager.transform, false)
            );
            blow.name = "Blow";
            blow.transform.rotation = LookAtMouse();
            Blow script = blow.GetComponent<Blow>();
            script.stats = weapon.stats;
            script.movement = distance.normalized * -1;
            script.Reset();
            yield return new WaitForSeconds(weapon.stats.Life);
            blow.SetActive(false);
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
