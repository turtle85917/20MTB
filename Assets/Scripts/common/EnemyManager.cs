using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    [SerializeField] private GameObject Enemies;
    [SerializeField] private EnemyData[] enemies;
    private List<EnemyPool> enemyPools;

    public void NewEnemy(string enemyId)
    {
        EnemyData enemyData = Array.Find(enemies, item => item.enemyId == enemyId);
        GameObject enemy = Instantiate(enemyData.Prefab, Enemies.transform, false);
        enemy.name = "Enemy" + Enemies.transform.childCount;
        enemy.transform.position = FollowCamera.instance.MovePosition(Player.instance.transform.position + (Vector3)UnityEngine.Random.insideUnitCircle * 20, 0);
        enemy.GetComponent<Enemy>().enemy = enemyData;
        enemyPools.Add(new EnemyPool(){
            target = enemy,
            health = enemyData.stats.MaxHealth,
            data = enemyData
        });
    }

    public EnemyPool GetEnemy(GameObject Object)
    {
        return enemyPools.Find(item => item.target.Equals(Object));
    }

    private void Awake()
    {
        instance = this;
        enemyPools = new(){};
    }
}
