using System.Collections;
using UnityEngine;

public class DefaultWeapon : MonoBehaviour
{
    [SerializeField] private GameObject PlayerEffects;
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
            GameObject blow = Instantiate(Blow, PlayerEffects.transform, false);
            blow.name = "Blow";
            Blow script = blow.GetComponent<Blow>();
            script.stats = weapon.stats;
            script.movement = distance.normalized * -1;
            script.Reset();
            yield return new WaitForSeconds(weapon.stats.Life);
            blow.SetActive(false);
        }
    }
}
