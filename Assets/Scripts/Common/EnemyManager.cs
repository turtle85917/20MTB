using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance {get; private set;}
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
            moveSpeed = UnityEngine.Random.Range(enemyData.stats.MinMoveSpeed, enemyData.stats.MaxMoveSpeed),
            enemyWeapons = new(){},
            data = enemyData
        });
        return enemy;
    }

    public void AddWeaponToEnemy(GameObject target, string weaponId)
    {
        EnemyPool enemyPool = GetEnemy(target);
        Weapon weapon = WeaponBundle.GetWeapon(weaponId);
        enemyPool.enemyWeapons.Add(new EnemyWeapon(){
            type = "N",
            weapon = weapon.weapon,
            stats = weapon.stats,
            count = 0
        });
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
