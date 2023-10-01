using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemBundle : MonoBehaviour
{
    public List<Item> assets;
    [SerializeField] private ItemData[] itemDatas;

    private void Start()
    {
        var excelDatas = CSVReader.Read("Weapons");
        for(int i = 0; i < excelDatas.Count; i++)
        {
            var excelData = excelDatas[i];
            Item item = new Item()
            {
                type = (string)excelData["Type"],
                item = itemDatas[i],
                stats = new ItemStats()
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
            assets.Add(item);
        }
    }

    private object TryGetNumValue(object value)
    {
        if(value is null) return 0;
        return value;
    }
}
