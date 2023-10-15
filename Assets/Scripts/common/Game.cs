using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public Character character;
    public PlayerData playerData;
    public GameObject PoolManager;
    public GameObject Exp;
    public GameObject Sturn;
    public static List<Weapon> playerWeapons;
    public static Game instance {get; private set;}
    [SerializeField] private PlayerData[] players;
    [SerializeField] private Image HeadImage;
    [SerializeField] private TMP_Text TimerText;
    [SerializeField] private Slider HealthBar;
    [SerializeField] private TMP_Text HealthText;
    [SerializeField] private Slider ExpBar;
    [SerializeField] private TMP_Text LevelText;
    [SerializeField] private GameObject[] WeaponSlots;
    [SerializeField] private GameObject WeaponSlotPanel;
    [SerializeField] private Cycle cycle;
    private List<int> times;
    private int timer = 20 * 60; // 기본 20분
    private int maxTime = 20 * 60;

    public void AddWeapon(string weaponId)
    {
        Weapon weapon = WeaponBundle.GetWeapon(weaponId);
        playerWeapons.Add(weapon);
        foreach(GameObject WeaponSlot in WeaponSlots)
        {
            if(WeaponSlot.transform.childCount == 0)
            {
                GameObject weaponSlot = Instantiate(WeaponSlotPanel, WeaponSlot.transform, false);
                weaponSlot.name = "Logo";
                weaponSlot.GetComponent<Image>().sprite = weapon.weapon.logo;
                if(weapon.type != "D")
                {
                    IExecuteWeapon executeWeapon = weaponSlot.AddComponent(weapon.weapon.executeWeapon.GetClass()) as IExecuteWeapon;
                    executeWeapon.ExecuteWeapon(weapon.weapon.resources, weapon.stats);
                }
                break;
            }
        }
    }

    public int GetDamage(int Power)
    {
        return Power + playerData.stats.Power / Power;
    }

    public void AttackEnemy(GameObject target, WeaponStats stats, int through, bool knockback = false, GameObject knocbackTarget = null)
    {
        bool critical = UnityEngine.Random.Range(0f, 1f) <= stats.CriticalHit;
        EnemyPool enemyPool = EnemyManager.instance.GetEnemy(target);
        if(knockback)
            enemyPool.target.GetComponent<Enemy>().Knockback(knocbackTarget ?? Player.instance.gameObject);
        int deal = GetDamage(critical ? stats.CriticalDamage : stats.Power) - through * stats.DecreasePower;
        enemyPool.health -= deal;
        Damage.instance.WriteDamage(target, deal, critical);
    }

    private void Awake()
    {
        instance = this;
        playerData = players[(int)character];
        playerWeapons = new(){};
        times = new(){};
    }

    private void Start()
    {
        HeadImage.sprite = playerData.headImage;
        AddWeapon(playerData.defaultWeapon);
        AddWeapon("Axe");
        StartCoroutine(Timer());
    }

    private void Update()
    {
        TimerText.text = TimeSpan.FromSeconds(timer).ToString(@"mm\:ss");
        HealthBar.value = (float)Player.health / playerData.stats.MaxHealth;
        HealthText.text = Player.health + " / " + playerData.stats.MaxHealth;
        ExpBar.value = (float)Player.exp / GetNeedExpFromLevel();
        LevelText.text = "Lv " + Player.level;
        foreach(CycleTimeline timeline in cycle.cycleTimelines)
        {
            if(maxTime - timer == timeline.time && !times.Contains(timeline.time))
            {
                times.Add(timeline.time);
                for(int i = 0; i < UnityEngine.Random.Range(timeline.spawnCount[0], timeline.spawnCount[1]); i++)
                {
                    EnemyData enemyData = timeline.enemies[UnityEngine.Random.Range(0, timeline.enemies.Length)];
                    GameObject enemy = EnemyManager.instance.NewEnemy(enemyData.enemyId);
                    if(timeline.circleRadius > 0)
                    {
                        enemy.transform.position = FollowCamera.instance.MovePosition(Player.instance.transform.position + (Vector3)UnityEngine.Random.insideUnitCircle.normalized * timeline.circleRadius, 0);
                        // EnemyManager.instance.AddWeaponToEnemy(enemy, "Axe");
                    }
                    else
                    {
                        if(timeline.spawnPosition.Length == 1)
                        {
                            enemy.transform.position = timeline.spawnPosition[0];
                        }
                        else
                        {
                            enemy.transform.position = new Vector3(
                                UnityEngine.Random.Range(timeline.spawnPosition[0].x, timeline.spawnPosition[1].x),
                                UnityEngine.Random.Range(timeline.spawnPosition[0].y, timeline.spawnPosition[1].y)
                            );
                        }
                    }
                }
            }
        }
    }

    private IEnumerator Timer()
    {
        while(timer > 0)
        {
            yield return new WaitForSeconds(1f);
            timer -= 1;
        }
    }

    private int GetNeedExpFromLevel()
    {
        return 50 * Player.level + 10;
    }
}
