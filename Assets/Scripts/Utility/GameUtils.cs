using System;
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

        public static int GetDirectionFromTarget(GameObject target)
        {
            if(target.CompareTag("Player")){
                int x = (int)Player.instance.Movement.x;
                return x == 0 ? 1 : x;
            }
            return target.transform.position.x > Player.instance.Movement.x ? 1 : -1;
        }

        public static Quaternion LookAtTarget(Vector2 origin, Vector2 target)
        {
            Vector2 distance = origin.x < target.x
                ? target - origin
                : origin - target
            ;
            return Quaternion.AngleAxis((float)(Math.Atan2(distance.y, distance.x) * Mathf.Rad2Deg), Vector3.forward);
        }
    }
}
