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

public enum PresentType
{
    Exp,
    DonatedBox
}

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject exp;
    [SerializeField] private GameObject donatedBox;
    [SerializeField] private GameObject Enemies;
    [SerializeField] private EnemyData[] enemies;
    private static EnemyManager instance;
    private int killStack;
    private List<EnemyPool> enemyPools;

    public static GameObject NewEnemy(string enemyId, string twitchUserId = null)
    {
        EnemyData enemyData = Array.Find(instance.enemies, item => item.enemyId == enemyId);
        GameObject enemy = ObjectPool.Get(instance.Enemies, enemyData.Prefab.name, enemyData.Prefab);
        EnemyPool enemyPool = new EnemyPool(){
            target = enemy,
            health = enemyData.stats.MaxHealth,
            moveSpeed = enemyData.stats.MoveSpeed * (UnityEngine.Random.value * 0.5f + 1),
            twitchUserId = twitchUserId,
            weapon = null,
            data = enemyData
        };
        enemy.GetComponent<EnemyAIStruct>().enemyPool = enemyPool;
        instance.enemyPools.Add(enemyPool);
        return enemy;
    }
    public static GameObject NewRandomEnemy()
    {
        EnemyData enemy = instance.enemies[UnityEngine.Random.Range(0, instance.enemies.Length)];
        return NewEnemy(enemy.enemyId);
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

    public static void DropPresent(EnemyPool enemyPool)
    {
        instance.killStack++;
        if(instance.killStack >= 4)
        {
            instance.killStack = 0;
            if(UnityEngine.Random.value < 0.3)
            {
                instance.DropPresent(enemyPool, PresentType.DonatedBox);
            }
        }
        else if(UnityEngine.Random.value < 0.6f)
            instance.DropPresent(enemyPool, PresentType.Exp);
    }

    private void DropPresent(EnemyPool enemyPool, PresentType present)
    {
        switch(present)
        {
            case PresentType.Exp:
                GameObject exp = ObjectPool.Get(Game.PoolManager, "Exp", instance.exp);
                exp.transform.position = enemyPool.target.transform.position;
                exp.GetComponent<Present>().exp = enemyPool.data.stats.Exp;
                break;
            case PresentType.DonatedBox:
                GameObject donatedBox = ObjectPool.Get(Game.PoolManager, "DonatedBox", instance.donatedBox);
                donatedBox.transform.position = enemyPool.target.transform.position;
                break;
        }
    }

    private void Awake()
    {
        instance = this;
        enemyPools = new(){};
    }
}
