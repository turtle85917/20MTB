using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    [SerializeField] private GameObject Enemies;
    [SerializeField] private EnemyData[] enemies;
    private List<EnemyPool> enemyPools;

    public GameObject NewEnemy(string enemyId)
    {
        EnemyData enemyData = Array.Find(enemies, item => item.enemyId == enemyId);
        GameObject enemy = Instantiate(enemyData.Prefab, Enemies.transform, false);
        enemy.name = "Enemy" + Enemies.transform.childCount;
        enemyPools.Add(new EnemyPool(){
            target = enemy,
            health = enemyData.stats.MaxHealth,
            MoveSpeed = UnityEngine.Random.Range(enemyData.stats.MoveSpeed[0], enemyData.stats.MoveSpeed[1]),
            data = enemyData
        });
        return enemy;
    }

    public EnemyPool GetEnemy(GameObject Object)
    {
        return enemyPools.Find(item => item.target.Equals(Object));
    }

    public void RemoveEnemy(EnemyPool pool)
    {
        enemyPools.Remove(pool);
    }

    private void Awake()
    {
        instance = this;
        enemyPools = new(){};
    }
}
