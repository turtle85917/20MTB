using System;
using System.Collections.Generic;
using _20MTB.Stats;
using _20MTB.Utillity;
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
        public int moveSpeed;
        public List<Weapon> enemyWeapons;
        public EnemyData data;
    }

    public static GameObject NewEnemy(string enemyId)
    {
        EnemyData enemyData = Array.Find(instance.enemies, item => item.enemyId == enemyId);
        GameObject enemy = Instantiate(enemyData.Prefab, instance.Enemies.transform, false);
        enemy.name = "Enemy";
        instance.enemyPools.Add(new EnemyPool(){
            target = enemy,
            health = enemyData.stats.MaxHealth,
            moveSpeed = UnityEngine.Random.Range(enemyData.stats.MinMoveSpeed, enemyData.stats.MaxMoveSpeed),
            enemyWeapons = new(){},
            data = enemyData
        });
        return enemy;
    }

    public static void AddWeaponToEnemy(GameObject target, string weaponId)
    {
        EnemyPool enemyPool = GetEnemy(target);
        Weapon weapon = WeaponBundle.GetWeapon(weaponId);
        if(weapon.type == "D") return;
        IExecuteWeapon executeWeapon = target.AddComponent(weapon.weapon.weaponCycleScriptFile.GetClass()) as IExecuteWeapon;
        executeWeapon.ExecuteWeapon(enemyPool.target);
        enemyPool.enemyWeapons.Add(new Weapon(){
            type = "N",
            weapon = weapon.weapon,
            stats = weapon.stats
        });
    }

    public static EnemyPool GetEnemy(GameObject Object)
    {
        return instance.enemyPools.Find(item => item.target.Equals(Object));
    }

    public static void RemoveEnemy(EnemyPool pool)
    {
        instance.enemyPools.Remove(pool);
    }

    public static void AttackEnemy(GameObject target, WeaponStats stats, int through = 1, Action<Enemy> processFunc = null)
    {
        EnemyPool enemyPool = GetEnemy(target);
        if(enemyPool == null) return;
        var res = CalcStat.GetDamageValueFromPlayerStat(stats, through);
        enemyPool.health -= res.demage;
        TextManager.WriteDamage(target, res.demage, res.isCritical);
        if(processFunc != null)
        {
            processFunc(target.GetComponent<Enemy>());
        }
    }

    private void Awake()
    {
        instance = this;
        enemyPools = new(){};
    }
}
