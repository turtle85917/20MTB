using System.Collections.Generic;
using UnityEngine;
using _20MTB.Utillity;
using UnityEngine.UI;
using System;

public class WeaponBundle : MonoBehaviour
{
    private static WeaponBundle instance;
    private static List<Weapon> assets;
    [Header("무기 스크립터블")]
    [SerializeField] private WeaponData[] weaponDatas;
    [Header("UI - Weapon Slot (Player)")]
    [SerializeField] private GameObject[] Slots;
    [SerializeField] private GameObject SlotPrefab;

    public static Weapon GetWeapon(string weaponId)
    {
        return assets.Find(item => item.weapon.weaponId.ToLower() == weaponId.ToLower());
    }

    public static Weapon GetWeaponByName(string weaponName)
    {
        return assets.Find(item => item.name.Equals(weaponName)) ?? GetWeapon(weaponName); // 이름을 구할 수 없을 경우, id로도 구해본다.
    }

    public static void AddWeaponToTarget(GameObject target, string weaponId)
    {
        Weapon weapon = GetWeapon(weaponId);
        Type monoscript = weapon.weapon.weaponCycleScriptFile.GetClass();
        if(target.CompareTag("Player")) // 플레이어
        {
            if(Player.playerData.weapons.Count == 6) return; // 무기 최대 6개까지 소지 가능
            GameObject slot = null;
            foreach(GameObject Slot in instance.Slots)
            {
                if(Slot.transform.childCount == 0)
                {
                    slot = Slot;
                    break;
                }
            }
            GameObject weaponSlot = Instantiate(instance.SlotPrefab, slot.transform, false);
            weaponSlot.name = "Logo";
            weaponSlot.GetComponent<Image>().sprite = weapon.weapon.logo;
            BaseCycle baseCycle = Activator.CreateInstance(monoscript) as BaseCycle;
            instance.StartCoroutine(baseCycle.Cycle(target));
        }
        else                            // 적
        {
            EnemyManager.EnemyPool enemyPool = EnemyManager.GetEnemy(target);
            enemyPool.weapon = new Weapon()
            {
                type = "N",
                weapon = weapon.weapon,
                stats = weapon.stats
            };
            BaseCycle baseCycle = Activator.CreateInstance(monoscript) as BaseCycle;
            instance.StartCoroutine(baseCycle.Cycle(enemyPool.target));
        }
    }

    private void Awake()
    {
        instance = this;
        assets = new List<Weapon>(){};
        var excelDatas = CSVReader.Read("Weapons");
        for(int i = 0; i < excelDatas.Count; i++)
        {
            var excelData = excelDatas[i];
            Weapon weapon = new Weapon()
            {
                type = (string)excelData["Type"],
                name = (string)excelData["Name"],
                weapon = weaponDatas[i],
                stats = new _20MTB.Stats.WeaponStats()
                {
                    Power = (int)excelData["Power"],
                    Cooldown = (int)excelData["Cooldown"],
                    Penetrate = (int)excelData["Penetrate"],
                    DecreasePower = (int)excelData["DecreasePower"],
                    Range = (int)excelData["Range"],
                    Life = (float)excelData["Life"],
                    CriticalHit = (float)excelData["CriticalHit"],
                    CriticalDamage = (int)excelData["CriticalDamage"],
                    ProjectileName = (string)excelData["ProjectileName"],
                    ProjectileSize = (int)TryGetNumValue(excelData["ProjectileSize"]),
                    ProjectileSpeed = (int)TryGetNumValue(excelData["ProjectileSpeed"]),
                    ProjectileCount = (int)TryGetNumValue(excelData["ProjectileCount"]),
                    Count = (int)TryGetNumValue(excelData["Count"])
                }
            };
            assets.Add(weapon);
        }
    }

    private object TryGetNumValue(object value)
    {
        if(value is null) return 0;
        return value;
    }
}
