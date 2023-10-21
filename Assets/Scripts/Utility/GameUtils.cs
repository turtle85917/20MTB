using UnityEngine;

namespace _20MTB.Utillity
{
    public static class GameUtils
    {
        public static string GetAttackTargetTag(GameObject target)
        {
            return target.CompareTag("Player")
                ? "Enemy"
                : "Player"
            ;
        }
    }
}
