using System;
using System.Collections;
using System.Collections.Generic;
using _20MTB.Utillity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public Character character;
    public GameObject PoolManager;
    public GameObject Exp;
    public static List<Weapon> playerWeapons;
    public static PlayerStatus playerData {get; private set;}
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
    private List<int> times;        // 소화된 시간 기록
    private int timer = 20 * 60;    // 기본 20분
    private readonly int maxTime = 20 * 60;
    private readonly Vector2 maxPoint = new Vector2(29.6f, 17.8f);
    private readonly Vector2 minPoint = new Vector2(-29.6f, -16f);
    #region 플레이어 데이터
    public class PlayerStatus
    {
        public int health;
        public int level;
        public int exp;
        public PlayerData data;
    }
    #endregion

    public static Vector3 MovePositionLimited(Vector2 position, float z)
    {
        return new(Math.Max(instance.minPoint.x, Math.Min(position.x, instance.maxPoint.x)), Math.Max(instance.minPoint.y, Math.Min(position.y, instance.maxPoint.y)), z);
    }

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
                    IExecuteWeapon executeWeapon = weaponSlot.AddComponent(weapon.weapon.weaponCycleScriptFile.GetClass()) as IExecuteWeapon;
                    executeWeapon.ExecuteWeapon(weapon.weapon.resources, weapon.stats);
                }
                break;
            }
        }
    }

    private void Awake()
    {
        instance = this;
        PlayerData data = players[(int)character];
        playerData = new PlayerStatus(){
            health = data.stats.MaxHealth,
            level = 0,
            exp = 0,
            data = data
        };
        playerWeapons = new(){};
        times = new(){};
    }

    private void Start()
    {
        HeadImage.sprite = playerData.data.headImage;
        AddWeapon(playerData.data.defaultWeapon);
        // AddWeapon("Axe");
        StartCoroutine(Timer());
    }

    private void Update()
    {
        TimerText.text = TimeSpan.FromSeconds(timer).ToString(@"mm\:ss");
        HealthBar.value = (float)playerData.health / playerData.data.stats.MaxHealth;
        HealthText.text = playerData.health + " / " + playerData.data.stats.MaxHealth;
        ExpBar.value = (float)playerData.exp / CalcStat.GetNeedExpFromLevel();
        LevelText.text = "Lv " + playerData.level;
        SpawnEnemies();
    }

    private void LateUpdate()
    {
        Vector3 oldPosition = Player.instance.transform.position;
        oldPosition.z = Camera.main.transform.position.z;
        Camera.main.transform.position = oldPosition;
    }

    private void SpawnEnemies()
    {
        foreach(CycleTimeline timeline in cycle.cycleTimelines)
        {
            if(maxTime - timer == timeline.time && !times.Contains(timeline.time))
            {
                times.Add(timeline.time);
                for(int i = 0; i < UnityEngine.Random.Range(timeline.spawnCount[0], timeline.spawnCount[1]); i++)
                {
                    EnemyData enemyData = timeline.enemies[UnityEngine.Random.Range(0, timeline.enemies.Length)];
                    GameObject enemy = EnemyManager.NewEnemy(enemyData.enemyId);
                    if(timeline.circleRadius > 0)
                    {
                        enemy.transform.position = MovePositionLimited(Player.instance.transform.position + (Vector3)UnityEngine.Random.insideUnitCircle.normalized * timeline.circleRadius, 0);
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
}
