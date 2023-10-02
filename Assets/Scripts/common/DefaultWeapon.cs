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
            GameObject blow = Instantiate(Blow, PlayerEffects.transform, false);
            blow.name = "Blow";
            Blow script = blow.GetComponent<Blow>();
            script.stats = weapon.stats;
            script.movement = Player.instance.lastMovement;
            script.Reset();
            yield return new WaitForSeconds(weapon.stats.Life);
            blow.SetActive(false);
        }
    }
}
