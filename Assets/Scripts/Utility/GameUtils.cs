using System;
using UnityEngine;

namespace _20MTB.Utillity
{
    public static class GameUtils
    {
        public static readonly Vector2 maxPosition = new Vector2(29.72f, 17.56f);
        public static readonly Vector2 minPosition = new Vector2(-29.72f, -16.19f);

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
            return new(Math.Max(minPosition.x, Math.Min(position.x, maxPosition.x)), Math.Max(minPosition.y, Math.Min(position.y, maxPosition.y)), position.z);
        }

        public static int GetNeedExpFromLevel()
        {
            return 20 * Mathf.Max(Player.playerData.level, 1);
        }

        public static GameObject GetWeaponsObject(GameObject parent)
        {
            Transform transform = parent.transform;
            for (int i = 0; i < transform.childCount; i++) 
            {
                if(transform.GetChild(i).CompareTag("Weapons"))
                {
                    return transform.GetChild(i).gameObject;
                }
            }
            return null;
        }

        public static Vector3 FixedPosition() => new Vector3(Player.@object.transform.position.x, Player.@object.transform.position.y, -10);
    }
}
