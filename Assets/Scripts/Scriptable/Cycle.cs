using System;
using UnityEngine;

[CreateAssetMenu(menuName = "사이클")]
public class Cycle : ScriptableObject
{
    public CycleTimeline[] cycleTimelines;
}

[Serializable]
public class CycleTimeline
{
    public int time;
    public Vector2[] spawnPosition;
    public int[] spawnCount;
    public int circleRadius;
    public EnemyData[] enemies;
}
