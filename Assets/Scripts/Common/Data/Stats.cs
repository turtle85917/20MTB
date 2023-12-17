using System;

namespace _20MTB.Stats
{
    #region 플레이어 기본 스탯
    [Serializable]
    public class Stats
    {
        public int MaxHealth;
        public int Power;
        public int MoveSpeed;
    }
    #endregion

    #region 적 기본 스탯
    [Serializable]
    public class EnemyStats
    {
        public int MaxHealth;
        public int Power;
        public float MoveSpeed;
        public int Exp;
    }
    #endregion

    #region 플레이어 무기 스탯
    public class WeaponStats
    {
        public int Power;
        public int Cooldown;
        public int Penetrate;
        public int DecreasePower;
        public int Range;
        public float Life;
        public float CriticalHit;
        public int CriticalDamage;
        public int ProjectileSize;
        public int ProjectileSpeed;
        public int ProjectileCount;
    }
    #endregion
}
