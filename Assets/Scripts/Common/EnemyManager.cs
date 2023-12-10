using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool
{
    public GameObject target;
    public float health;
    public float moveSpeed;
    public string twitchUserId;
    public Weapon weapon;
    public EnemyData data;
}

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject Enemies;
    [SerializeField] private EnemyData[] enemies;
    private static EnemyManager instance;
    private List<EnemyPool> enemyPools;

    public static GameObject NewEnemy(string enemyId, string twitchUserId = null)
    {
        EnemyData enemyData = Array.Find(instance.enemies, item => item.enemyId == enemyId);
        GameObject enemy = ObjectPool.Get(instance.Enemies, "Enemy", enemyData.Prefab);
        EnemyPool enemyPool = new EnemyPool(){
            target = enemy,
            health = enemyData.stats.MaxHealth,
            moveSpeed = UnityEngine.Random.Range(enemyData.stats.MinMoveSpeed, enemyData.stats.MaxMoveSpeed),
            twitchUserId = twitchUserId,
            weapon = null,
            data = enemyData
        };
        enemy.GetComponent<Enemy>().enemyPool = enemyPool;
        instance.enemyPools.Add(enemyPool);
        return enemy;
    }

    public static EnemyPool GetEnemy(GameObject enemy)
    {
        if(enemy.name == "Jinhe") return enemy.GetComponent<Jinhe>().weaponOwner;
        return instance.enemyPools.Find(item => item.target.Equals(enemy));
    }
    public static EnemyPool GetEnemy(string twitchUserId)
    {
        return instance.enemyPools.Find(item => item.twitchUserId == twitchUserId);
    }
    public static bool IsEnemyAlive(GameObject enemy) => GetEnemy(enemy) != null || GetEnemy(enemy)?.health <= 0;

    public static EnemyPool[] GetEnemies()
    {
        return instance.enemyPools.ToArray();
    }

    public static void RemoveEnemy(EnemyPool pool)
    {
        instance.enemyPools.Remove(pool);
    }

    private void Awake()
    {
        instance = this;
        enemyPools = new(){};
    }
}
