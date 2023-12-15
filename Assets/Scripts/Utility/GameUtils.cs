using System;
using UnityEngine;

namespace _20MTB.Utillity
{
    public static class GameUtils
    {
        private static readonly Vector2 maxPoint = new Vector2(29.72f, 17.56f);
        private static readonly Vector2 minPoint = new Vector2(-29.72f, -16.19f);

        public static string GetTargetTag(GameObject target)
        {
            return target.CompareTag("Player") ? "Enemy" : "Player";
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

        public static Vector3 MovePositionLimited(Vector3 position)
        {
            return new(Math.Max(minPoint.x, Math.Min(position.x, maxPoint.x)), Math.Max(minPoint.y, Math.Min(position.y, maxPoint.y)), position.z);
        }

        public static int GetNeedExpFromLevel()
        {
            return 50 * Mathf.Max(Player.playerData.level, 1);
        }

        public static GameObject FindGameObjectInChildWithTag(GameObject parent, string tag)
        {
            Transform transform = parent.transform;
            for (int i = 0; i < transform.childCount; i++) 
            {
                if(transform.GetChild(i).CompareTag(tag))
                {
                    return transform.GetChild(i).gameObject;
                }
            }
            return null;
        }

        public static Vector3 FixedPosition() => new Vector3(Player.@object.transform.position.x, Player.@object.transform.position.y, -10);
    }
}
