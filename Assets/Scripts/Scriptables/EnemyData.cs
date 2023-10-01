using UnityEngine;

[CreateAssetMenu(menuName = "적")]
public class EnemyData : ScriptableObject
{
    public string enemyId;
    public GameObject Prefab;
    public RuntimeAnimatorController controller;
    public Stats stats;
}
