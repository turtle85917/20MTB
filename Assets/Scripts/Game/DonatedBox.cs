using System.Collections;
using System.Linq;
using _20MTB.Utillity;
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

    [Header("후원 열차")]
    [SerializeField] private Animator HypeTrainPanel;
    [SerializeField] private Slider HypeTrainMeter;
    [SerializeField] private TMP_Text HypeTrainMeterText;
    [SerializeField] private TMP_Text HypeTrainLevelText;
    [SerializeField] private Animator HypeTrainLevelUpPanel;

    private float endValue;
    private int combo;
    private float lastInputTime;
    private bool isPress;

    private readonly float delay = 0.15f;
    private readonly Color MeterColor = new Color(153f / 255f, 72f / 255f, 214f / 255f);
    private readonly string[] presentNames = new string[]{
        "체력",
        "최대 체력",
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
        combo = 0;
        isPress = true;
        DonatedBoxPanel.SetActive(true);
        PressSpace.SetActive(true);
        HypeTrainPanel.gameObject.SetActive(false);
        Game.Pause();
    }

    private void Start()
    {
        HypeTrainMeter.value = 0;
        HypeTrainMeterText.text = "0%";
        HypeTrainLevelText.text = "LVL 1";
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
                    Camera.main.transform.position = GameUtils.FixedPosition() + (Vector3)Random.insideUnitCircle * 0.5f;
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
            Camera.main.transform.position = GameUtils.FixedPosition();
            ((RectTransform)DonatedBoxPanel.transform).anchoredPosition = Vector2.zero;
        }
        if(combo == 13 && isPress)
        {
            isPress = false;
            OpenBox();
            StartCoroutine(HypeTrain());
        }
        if(!Mathf.Approximately(endValue, HypeTrainMeter.value))
        {
            HypeTrainMeter.value = Mathf.Lerp(HypeTrainMeter.value, endValue, 0.1f);
        }
    }

    private void CheckLevelUp()
    {
        int maxMeter = GetMaxHypeTrainMeter();
        bool isLevelUp = false;
        while(Player.playerData.hypeTrain.meter >= maxMeter)
        {
            Player.playerData.hypeTrain.meter -= maxMeter;
            Player.playerData.hypeTrain.level++;
            isLevelUp = true;
        }
        if(isLevelUp)
        {
            HypeTrainLevelUpPanel.gameObject.SetActive(true);
            HypeTrainLevelUpPanel.SetTrigger("LevelUp");
        }
        HypeTrainLevelText.text = "LVL " + Player.playerData.hypeTrain.level;
        endValue = Player.playerData.hypeTrain.meter / (float)GetMaxHypeTrainMeter();
        HypeTrainMeterText.text = Mathf.Ceil(endValue * 100) + "%";
    }

    private void OpenBox()
    {
        PressSpace.SetActive(false);
        donatedBoxAnimator.SetTrigger("Open");
        Camera.main.transform.position = GameUtils.FixedPosition();
        ((RectTransform)DonatedBoxPanel.transform).anchoredPosition = Vector2.zero;

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

                int maxUpLevel = GetWeaponUpgradeLevel();
                Weapon playerWeapon = Player.playerData.weapons.Find(item => item.weapon.weaponId == decideWeapon.weapon.weaponId);
                if(playerWeapon == null)
                {
                    maxUpLevel -= 1;
                    WeaponBundle.AddWeaponToTarget(Player.@object, decideWeapon.weapon.weaponId);
                }
                for(int i = 0; i < Mathf.Min(maxUpLevel, decideWeapon.weapon.levels.Length - playerWeapon?.level ?? 0); i++)
                    WeaponBundle.UpgradeTargetsWeapon(Player.@object, decideWeapon.weapon.weaponId);

                Weapon GetRandomWeapon()
                {
                    Weapon[] weapons = WeaponBundle.GetWeapons(item => item.type != "D");
                    return weapons[Random.Range(0, weapons.Length)];
                }
                break;
            case ItemType.Present:
                int present = Random.Range(0, 4);
                int value = 0;
                switch(present)
                {
                    case 0:
                        value = Random.Range(3, 8);
                        Player.playerData.health += Mathf.RoundToInt(value * GetValueRatio());
                        break;
                    case 1:
                        value = 10;
                        Player.playerData.maxHealth += 10;
                        break;
                    case 2:
                        value = Random.Range(20, 50);
                        Player.playerData.exp += Mathf.RoundToInt(value * GetValueRatio());
                        break;
                    case 3:
                        value = Random.Range(100, 300);
                        Player.playerData.hypeTrain.meter += Mathf.RoundToInt(value * GetValueRatio());
                        break;
                }
                item.sprite = presentSprites[present];
                item.color = present == 2 ? MeterColor : Color.white;
                itemName.text = presentNames[present] + " +" + value;
                break;
        }
    }

    private IEnumerator HypeTrain()
    {
        yield return new WaitForSecondsRealtime(2.5f);
        if(Player.playerData.hypeTrain.beforeMeter != Player.playerData.hypeTrain.meter)
        {
            HypeTrainPanel.gameObject.SetActive(true);
            HypeTrainLevelUpPanel.gameObject.SetActive(false);
            HypeTrainPanel.SetTrigger("Open");
            Player.playerData.hypeTrain.beforeMeter = Player.playerData.hypeTrain.meter;
            CheckLevelUp();
        }
    }

    private float GetValueRatio() => Mathf.Log(Player.playerData.hypeTrain.level);

    private int GetWeaponUpgradeLevel()
    {
        int level = Player.playerData.hypeTrain.level;
        if(1 <= level && level <= 4) return 1;
        else if(4 < level && level <= 7) return 2;
        else if(7 < level && level <= 10) return 3;
        else if(10 < level && level <= 18) return 4;
        else return 5;
    }

    private int GetMaxHypeTrainMeter()
    {
        int result = 0;
        int level = Player.playerData.hypeTrain.level;
        for(int i = 0; i < level; i++) result += (level - i) * 1000;
        return result;
    }
}
