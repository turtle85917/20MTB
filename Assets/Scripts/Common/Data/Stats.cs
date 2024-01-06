using System;
using UnityEngine;

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

    #region 무기 스탯
    public class WeaponStats
    {
        public int Power;
        public float Cooldown;
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

    #region 이동속도 복제 관련 스탯
    public class MoveSpeedStats
    {
        public float originMoveSpeed;
        public float otherMoveSpeed;
        public float value
        {
            get
            {
                return originMoveSpeed * Mathf.Max(0.04f, otherMoveSpeed);
            }
        }
    }
    #endregion
}
