using System;
using UnityEngine;

namespace _20MTB.Utillity
{
    public static class GameUtils
    {
        private static readonly Vector2 maxPoint = new Vector2(29.6f, 17.8f);
        private static readonly Vector2 minPoint = new Vector2(-29.6f, -16f);

        public static WeaponUser GetWeaponUserType(GameObject target)
        {
            return target.CompareTag("Player")
                ? WeaponUser.Player
                : WeaponUser.Enemy
            ;
        }

        public static int GetDirectionFromTarget(GameObject target)
        {
            if(target.CompareTag("Player")){
                int x = (int)Player.lastDirection.x;
                return x == 0 ? 1 : x;
            }
            return target.transform.position.x > Player.lastDirection.x ? 1 : -1;
        }

        public static Quaternion LookAtTarget(Vector3 origin, Vector3 target)
        {
            Vector3 distance = target - origin;
            float angle = (float)(Math.Atan2(distance.y, distance.x) * Mathf.Rad2Deg);
            return Quaternion.AngleAxis(angle, Vector3.forward);
        }

        public static Vector3 MovePositionLimited(Vector3 position, float z)
        {
            return new(Math.Max(minPoint.x, Math.Min(position.x, maxPoint.x)), Math.Max(minPoint.y, Math.Min(position.y, maxPoint.y)), position.z);
        }
    }
}
