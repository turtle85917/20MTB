using System.Collections.Generic;
using UnityEngine;

public class WeaponBundle : MonoBehaviour
{
    public static List<Weapon> assets;
    [SerializeField] private WeaponData[] weaponDatas;

    public static Weapon GetWeapon(string weaponId)
    {
        return assets.Find(item => item.weapon.WeaponId == weaponId);
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
                weapon = weaponDatas[i],
                stats = new WeaponStats()
                {
                    Power = (int)excelData["Power"],
                    Charging = (int)excelData["Charging"],
                    Through = (int)excelData["Through"],
                    DecreasePower = (int)excelData["DecreasePower"],
                    CriticalHit = (float)excelData["CriticalHit"],
                    CriticalDamage = (int)excelData["CriticalDamage"],
                    ProjectileName = (string)excelData["ProjectileName"],
                    ProjectileSize = (int)TryGetNumValue(excelData["ProjectileSize"]),
                    ProjectileSpeed = float.Parse(TryGetNumValue(excelData["ProjectileSpeed"]).ToString()),
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
