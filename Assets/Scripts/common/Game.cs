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
    public static List<Weapon> playerWeapons;
    public static Game instance {get; private set;}
    [SerializeField] private PlayerData[] players;
    [SerializeField] private EnemyData[] enemies;
    [SerializeField] private GameObject Enemies;
    [SerializeField] private Image HeadImage;
    [SerializeField] private TMP_Text TimerText;
    [SerializeField] private Slider HealthBar;
    [SerializeField] private TMP_Text HealthText;
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
        // NOTE: 임시
        GameObject enemy = Instantiate(enemies[0].Prefab, Enemies.transform, false);
        enemy.name = "Enemy1";
        enemy.transform.position = new(0, 10, 0);
        enemy.GetComponent<Enemy>().enemy = enemies[0];
    }

    private void Update()
    {
        TimerText.text = TimeSpan.FromSeconds(timer).ToString(@"mm\:ss");
        HealthBar.value = (float)Player.health / playerData.stats.MaxHealth;
        HealthText.text = Player.health + " / " + playerData.stats.MaxHealth;
    }

    IEnumerator Timer()
    {
        while(timer > 0)
        {
            yield return new WaitForSeconds(1f);
            timer -= 1;
        }
    }
}
