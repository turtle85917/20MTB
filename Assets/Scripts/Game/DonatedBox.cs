using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DonatedBox : MonoBehaviour
{
    [SerializeField] private GameObject DonatedBoxPanel;
    [SerializeField] private Animator donatedBoxAnimator;
    [SerializeField] private GameObject PressSpace;
    [SerializeField] private Image item;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private GameObject CloseDesc;
    [SerializeField] private Sprite[] presentSprites;
    private int combo;
    private float lastInputTime;
    private bool isPress;
    private readonly float delay = 0.15f;
    private readonly Color MeterColor = new Color(153f / 255f, 72f / 255f, 214f / 255f);
    private readonly string[] presentNames = new string[]{
        "체력",
        "경험치",
        "후원 열차 미터"
    };

    private enum ItemType
    {
        Weapon,
        Present
    }

    public void Open()
    {
        DonatedBoxPanel.SetActive(true);
        PressSpace.SetActive(true);
        combo = 0;
        isPress = true;
        Game.Pause();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(isPress)
            {
                if(Time.time - lastInputTime < delay)
                {
                    combo++;
                    ((RectTransform)DonatedBoxPanel.transform).anchoredPosition = Random.insideUnitCircle * 3f * combo;
                }
                lastInputTime = Time.time;
            }
            if(CloseDesc.activeSelf)
            {
                DonatedBoxPanel.SetActive(false);
                Game.Resume();
            }
        }
        if(Input.GetKeyUp(KeyCode.Space) && isPress)
        {
            ((RectTransform)DonatedBoxPanel.transform).anchoredPosition = Vector2.zero;
        }
        if(combo == 20 && isPress)
        {
            isPress = false;
            PressSpace.SetActive(false);
            donatedBoxAnimator.SetTrigger("Open");

            ItemType itemType;
            if(Random.value < 0.4f) itemType = ItemType.Present;
            else itemType = ItemType.Weapon;
            switch(itemType)
            {
                case ItemType.Weapon:
                    Weapon[] usableWeapons = Player.playerData.weapons.Where(item => item.weapon.levels.Length > item.level).ToArray();
                    Weapon decideWeapon = null;
                    float random = 0.6f;
                    if(usableWeapons.Length == 0) decideWeapon = GetRandomWeapon();
                    else if(usableWeapons.Length == 6 || Random.value > random) decideWeapon = usableWeapons[Random.Range(0, usableWeapons.Length)];
                    else decideWeapon = GetRandomWeapon();
                    item.sprite = decideWeapon.weapon.logo;
                    item.color = Color.white;
                    itemName.text = decideWeapon.name;

                    bool hasWeapon = Player.playerData.weapons.Exists(item => item.weapon.weaponId == decideWeapon.weapon.weaponId);
                    if(hasWeapon)
                    {
                        WeaponBundle.UpgradeTargetsWeapon(Player.@object, decideWeapon.weapon.weaponId);
                    }
                    else
                    {
                        WeaponBundle.AddWeaponToTarget(Player.@object, decideWeapon.weapon.weaponId);
                    }

                    Weapon GetRandomWeapon()
                    {
                        Weapon[] weapons = WeaponBundle.GetWeapons(item => item.type != "D");
                        return weapons[Random.Range(0, weapons.Length)];
                    }
                    break;
                case ItemType.Present:
                    int present = Random.Range(0, 3);
                    int value = 0;
                    switch(present)
                    {
                        case 0:
                            value = Random.Range(3, 8);
                            Player.playerData.health += value;
                            break;
                        case 1:
                            value = Random.Range(20, 50);
                            Player.playerData.exp += value;
                            break;
                        case 2:
                            value = Random.Range(100, 300);
                            break;
                    }
                    item.sprite = presentSprites[present];
                    item.color = present == 2 ? MeterColor : Color.white;
                    itemName.text = presentNames[present] + " +" + value;
                    break;
            }
        }
    }
}
