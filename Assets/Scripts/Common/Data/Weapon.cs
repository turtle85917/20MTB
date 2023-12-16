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
                Power = Mathf.CeilToInt(stats.Power * decrease),
                Cooldown = Mathf.CeilToInt(stats.Cooldown * decrease),
                Penetrate = Mathf.CeilToInt(stats.Penetrate * decrease),
                DecreasePower = Mathf.CeilToInt(stats.DecreasePower * decrease),
                Range = stats.Range,
                Life = stats.Life * decrease,
                CriticalHit = stats.CriticalHit * decrease,
                CriticalDamage = Mathf.CeilToInt(stats.CriticalDamage * decrease),
                ProjectileSize = stats.ProjectileSize,
                ProjectileSpeed = stats.ProjectileSpeed,
                ProjectileCount = stats.ProjectileCount
            }
        };
    }
}
