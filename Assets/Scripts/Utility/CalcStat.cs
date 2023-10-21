using _20MTB.Stats;

namespace _20MTB.Utillity
{
    public static class CalcStat
    {
        public class DamageResult
        {
            public int demage;
            public bool isCritical;
        }

        public static int GetDamageValueFromPlayerStat(int Power)
        {
            return Power + Game.playerData.data.stats.Power;
        }

        public static DamageResult GetDamageValueFromPlayerStat(WeaponStats stats, int through)
        {
            bool isCritical = CheckCriticalHit(stats.CriticalHit);
            return new DamageResult(){
                demage = (isCritical ? stats.CriticalDamage : stats.Power) - stats.DecreasePower * through,
                isCritical = isCritical
            };
        }

        public static int GetDamageValueFromEnemyStat(int enemyPower, int Power)
        {
            return Power + enemyPower;
        }

        public static int GetNeedExpFromLevel()
        {
            return 50 * Game.playerData.level + 10;
        }

        public static bool CheckCriticalHit(float criticalHit)
        {
            return UnityEngine.Random.value < criticalHit;
        }
    }
}
