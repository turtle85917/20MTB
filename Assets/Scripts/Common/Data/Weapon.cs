using _20MTB.Stats;
using UnityEngine;

public class Weapon
{
    public string type;
    public string name;
    public int level;
    public WeaponData weapon;
    public WeaponStats stats;

    public Weapon Copy(float decrease)
    {
        return new Weapon()
        {
            type = type,
            name = name,
            weapon = weapon,
            level = 1,
            stats = new WeaponStats()
            {
                Power = Mathf.RoundToInt(stats.Power * decrease),
                Cooldown = Mathf.RoundToInt(stats.Cooldown * decrease),
                Penetrate = Mathf.RoundToInt(stats.Penetrate * decrease),
                DecreasePower = Mathf.RoundToInt(stats.DecreasePower * decrease),
                Range = stats.Range,
                Life = Mathf.RoundToInt(stats.Life * decrease),
                CriticalHit = Mathf.RoundToInt(stats.CriticalHit * decrease),
                CriticalDamage = Mathf.RoundToInt(stats.CriticalDamage * decrease),
                ProjectileSize = stats.ProjectileSize,
                ProjectileSpeed = stats.ProjectileSpeed,
                ProjectileCount = stats.ProjectileCount,
                Count = stats.Count
            }
        };
    }
}
