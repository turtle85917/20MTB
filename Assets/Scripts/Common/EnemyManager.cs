using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject Enemies;
    [SerializeField] private EnemyData[] enemies;
    private static EnemyManager instance;
    private List<EnemyPool> enemyPools;
    public class EnemyPool
    {
        public GameObject target;
        public int health;
        public float moveSpeed;
        public Weapon weapon;
        public EnemyData data;
    }

    public static GameObject NewEnemy(string enemyId)
    {
        EnemyData enemyData = Array.Find(instance.enemies, item => item.enemyId == enemyId);
        GameObject enemy = ObjectPool.Get(instance.Enemies, "Enemy", enemyData.Prefab);
        EnemyPool enemyPool = new EnemyPool(){
            target = enemy,
            health = enemyData.stats.MaxHealth,
            moveSpeed = UnityEngine.Random.Range(enemyData.stats.MinMoveSpeed, enemyData.stats.MaxMoveSpeed),
            weapon = null,
            data = enemyData
        };
        enemy.GetComponent<Enemy>().enemyPool = enemyPool;
        instance.enemyPools.Add(enemyPool);
        return enemy;
    }

    public static EnemyPool GetEnemy(GameObject Object)
    {
        GameObject target = Object;
        if(Object.name == "Jinhe")
            target = Object.GetComponent<Jinhe>().weaponUser;
        return instance.enemyPools.Find(item => item.target.Equals(target));
    }

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
