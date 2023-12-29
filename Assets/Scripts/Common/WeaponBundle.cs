using System.Collections.Generic;
using UnityEngine;
using _20MTB.Utillity;
using UnityEngine.UI;
using System;

public class WeaponBundle : MonoBehaviour
{
    public static WeaponBundle instance {get; private set;}
    private static List<Weapon> assets;
    [Header("무기 스크립터블")]
    [SerializeField] private WeaponData[] weaponDatas;
    [Header("UI - Weapon Slot (Player)")]
    [SerializeField] private GameObject[] Slots;
    [SerializeField] private GameObject SlotPrefab;

    public static Weapon[] GetWeapons(Predicate<Weapon> predicate)
    {
        return assets.FindAll(predicate).ToArray();
    }

    public static Weapon GetWeapon(string weaponId)
    {
        return assets.Find(item => item.weapon.weaponId.ToLower() == weaponId.ToLower());
    }
    public static Weapon GetWeaponFromTarget(string weaponId, GameObject weaponUser)
    {
        Weapon foundWeapon = null;
        if(weaponUser.CompareTag("Player")) foundWeapon = Player.playerData.weapons.Find(item => item.weapon.weaponId == weaponId);
        else foundWeapon = EnemyManager.GetEnemy(weaponUser)?.weapon;
        return foundWeapon;
    }
    public static Weapon GetWeaponByName(string weaponName)
    {
        return assets.Find(item => item.name.Equals(weaponName)) ?? GetWeapon(weaponName); // 이름을 구할 수 없을 경우, id로도 구해본다.
    }

    public static void AddWeaponToTarget(GameObject target, string weaponId)
    {
        Weapon weapon = GetWeapon(weaponId);
        if(weapon == null)
        {
            Debug.Log("Not found weapon");
            return;
        }
        Type monoscript = weapon.weapon.weaponCycleScriptFile ? weapon.weapon.weaponCycleScriptFile.GetClass() : null;
        if(target.CompareTag("Player"))
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
            Image image = weaponSlot.GetComponent<Image>();
            image.sprite = weapon.weapon.logo;
            image.maskable = weapon.weapon.isFullLogo;
            Player.playerData.weapons.Add(weapon.Copy(1));
            if(monoscript != null)
            {
                BaseCycle baseCycle = Activator.CreateInstance(monoscript) as BaseCycle;
                instance.StartCoroutine(baseCycle.Cycle(target));
            }
        }
        else
        {
            EnemyPool enemyPool = EnemyManager.GetEnemy(target);
            enemyPool.weapon = weapon.Copy(0.4f);
            BaseCycle baseCycle = Activator.CreateInstance(monoscript) as BaseCycle;
            instance.StartCoroutine(baseCycle.Cycle(enemyPool.target));
        }
    }

    public static void UpgradeTargetsWeapon(GameObject target, string weaponId)
    {
        Weapon weapon = GetWeaponFromTarget(weaponId, target);
        if(weapon == null)
        {
            Debug.Log("Target not has weapon.");
            return;
        }
        if(weapon.level == weapon.weapon.levels.Length)
        {
            Debug.Log("Target's weapon is max level");
            return;
        }
        WeaopnIncreaseStat weaopnIncreaseStat = weapon.weapon.levels[weapon.level++];
        foreach(var field in weaopnIncreaseStat.GetType().GetFields())
        {
            var weaponField = weapon.stats.GetType().GetField(field.Name);
            if(field.FieldType.Name == "Single")
            {
                weaponField.SetValue(weapon.stats, (float)weaponField.GetValue(weapon.stats) + (float)field.GetValue(weaopnIncreaseStat));
            }
            else
            {
                weaponField.SetValue(weapon.stats, (int)weaponField.GetValue(weapon.stats) + (int)field.GetValue(weaopnIncreaseStat));
            }
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
                    Cooldown = (float)excelData["Cooldown"],
                    Penetrate = (int)excelData["Penetrate"],
                    DecreasePower = (int)excelData["DecreasePower"],
                    Range = (int)excelData["Range"],
                    Life = (float)excelData["Life"],
                    CriticalHit = (float)excelData["CriticalHit"],
                    CriticalDamage = (int)excelData["CriticalDamage"],
                    ProjectileSize = (int)TryGetNumValue(excelData["ProjectileSize"]),
                    ProjectileSpeed = (int)TryGetNumValue(excelData["ProjectileSpeed"]),
                    ProjectileCount = (int)TryGetNumValue(excelData["ProjectileCount"])
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
