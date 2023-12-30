using System.Collections;
using UnityEngine;

public class Parachute : MonoBehaviour
{
    [HideInInspector] public string giftType;
    [HideInInspector] public string weaponName;
    [SerializeField] private Sprite heart;
    private new Animation animation;

    private void Awake()
    {
        animation = GetComponent<Animation>();
    }

    private void OnEnable()
    {
        animation.Play("Parachute_Idle");
        StartCoroutine(IEHide());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.name == "Player")
        {
            Sprite icon = null;
            string name = string.Empty;
            switch(giftType)
            {
                case "boom":
                    name = "꽝";
                    break;
                case "health":
                    int value = Random.Range(1, 6);
                    icon = heart;
                    name = "체력 + " + value;
                    Player.playerData.health += value;
                    break;
                case "enemy":
                    name = "적 + 2";
                    EnemyManager.NewEnemy("Bat");
                    EnemyManager.NewEnemy("Bat");
                    break;
                case "weapon":
                    Weapon weapon = WeaponBundle.GetWeaponByName(weaponName);
                    icon = weapon.weapon.logo;
                    name = weapon.name;

                    Weapon playerWeapon = WeaponBundle.GetWeaponFromTarget(weapon.weapon.weaponId, Player.@object);
                    if(playerWeapon == null) WeaponBundle.AddWeaponToTarget(Player.@object, weapon.weapon.weaponId);
                    else WeaponBundle.UpgradeTargetsWeapon(Player.@object, playerWeapon.weapon.weaponId);
                    break;
            }
            Game.instance.drops.ParachutePanel.transform.localPosition = (Vector2)Player.@object.transform.position;
            Game.instance.drops.ParachutePanel.gameObject.SetActive(true);
            Game.instance.drops.Icon.gameObject.SetActive(icon != null);
            Game.instance.drops.Icon.sprite = icon;
            Game.instance.drops.Name.text = name;
            Game.instance.drops.HideParachutePanel();
            Game.instance.drops.ParachutePanel.Play("ParachutePanel_Show");
            gameObject.SetActive(false);
        }
    }

    private IEnumerator IEHide()
    {
        yield return new WaitForSeconds(4f);
        gameObject.SetActive(false);
    }
}
