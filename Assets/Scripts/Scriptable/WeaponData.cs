using UnityEngine;

[System.Serializable]
public class WeaopnIncreaseStat
{
    public int Power;
    public float Cooldown;
    public int Penetrate;
    public int DecreasePower;
    public int Range;
    public float Life;
    public float CriticalHit;
    public int ProjectileSize;
    public int ProjectileSpeed;
    public int ProjectileCount;
}

[CreateAssetMenu(menuName = "무기")]
public class WeaponData : ScriptableObject
{
    public string weaponId;
    [TextArea] public string playerDescription;
    [TextArea] public string enemyDescription;
    public Sprite logo;
    public string weaponCycleName;
    public Object[] resources;
    public WeaopnIncreaseStat[] levels;
    public bool isFullLogo;
}
