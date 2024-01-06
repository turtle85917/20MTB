using _20MTB.Stats;
using UnityEngine;

[CreateAssetMenu(menuName = "적")]
public class EnemyData : ScriptableObject
{
    public string enemyId;
    public GameObject Prefab;
    public EnemyStats stats;
    public string bossName;
    public Sprite bossSprite; // 보스 전용
}
