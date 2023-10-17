using System.Collections.Generic;
using UnityEngine;

public class WeaponBundle : MonoBehaviour
{
    private static List<Weapon> assets;
    [SerializeField] private WeaponData[] weaponDatas;

    public static Weapon GetWeapon(string weaponId)
    {
        return assets.Find(item => item.weapon.WeaponId == weaponId);
    }
    
    public static Weapon GetWeaponByName(string weaponName)
    {
        return assets.Find(item => item.name.Equals(weaponName)) ?? GetWeapon(weaponName); // 이름을 구할 수 없을 경우, id로도 구해본다.
    }

    private void Awake()
    {
        assets = new(){};
        var excelDatas = CSVReader.Read("Weapons");
        for(int i = 0; i < excelDatas.Count; i++)
        {
            var excelData = excelDatas[i];
            Weapon weapon = new Weapon()
            {
                type = (string)excelData["Type"],
                name = (string)excelData["Name"],
                weapon = weaponDatas[i],
                stats = new WeaponStats()
                {
                    Power = (int)excelData["Power"],
                    Cooldown = (int)excelData["Cooldown"],
                    Through = (int)excelData["Through"],
                    DecreasePower = (int)excelData["DecreasePower"],
                    Life = (int)excelData["Life"],
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
