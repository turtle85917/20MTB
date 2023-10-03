using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public Character character;
    public PlayerData playerData;
    public GameObject PoolManager;
    public GameObject Exp;
    public static List<Weapon> playerWeapons;
    public static Game instance {get; private set;}
    [SerializeField] private PlayerData[] players;
    [SerializeField] private Image HeadImage;
    [SerializeField] private TMP_Text TimerText;
    [SerializeField] private Slider HealthBar;
    [SerializeField] private TMP_Text HealthText;
    [SerializeField] private Slider ExpBar;
    [SerializeField] private TMP_Text LevelText;
    [SerializeField] private GameObject[] WeaponSlots;
    [SerializeField] private GameObject WeaponSlotPanel;
    private int timer = 20 * 60; // 기본 20분

    public void AddWeapon(string weaponId)
    {
        Weapon weapon = WeaponBundle.GetWeapon(weaponId);
        playerWeapons.Add(weapon);
        foreach(GameObject WeaponSlot in WeaponSlots)
        {
            if(WeaponSlot.transform.childCount == 0)
            {
                GameObject weaponSlot = Instantiate(WeaponSlotPanel, WeaponSlot.transform, false);
                weaponSlot.name = "Logo";
                weaponSlot.GetComponent<Image>().sprite = weapon.weapon.logo;
                break;
            }
        }
    }

    private void Awake()
    {
        instance = this;
        playerData = players[(int)character];
        playerWeapons = new(){};
    }

    private void Start()
    {
        HeadImage.sprite = playerData.headImage;
        AddWeapon(playerData.defaultWeapon);
        StartCoroutine(Timer());
        EnemyManager.instance.NewEnemy("Panzee");
        EnemyManager.instance.NewEnemy("Panzee");
        EnemyManager.instance.NewEnemy("Panzee");
        EnemyManager.instance.NewEnemy("Panzee");
        EnemyManager.instance.NewEnemy("Panzee");
    }

    private void Update()
    {
        TimerText.text = TimeSpan.FromSeconds(timer).ToString(@"mm\:ss");
        HealthBar.value = (float)Player.health / playerData.stats.MaxHealth;
        HealthText.text = Player.health + " / " + playerData.stats.MaxHealth;
        ExpBar.value = (float)Player.exp / GetNeedExpFromLevel();
        LevelText.text = "Lv " + Player.level;
    }

    private IEnumerator Timer()
    {
        while(timer > 0)
        {
            yield return new WaitForSeconds(1f);
            timer -= 1;
        }
    }

    private int GetNeedExpFromLevel()
    {
        return 50 * Player.level + 10;
    }
}
