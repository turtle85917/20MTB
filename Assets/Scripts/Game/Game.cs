using System.Collections;
using System.Collections.Generic;
using _20MTB.Utillity;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static GameObject Player;
    public static GameObject PoolManager;
    public static GameObject PlayerWeapons;
    public static Game instance {get; private set;}
    [SerializeField] private PlayerData[] players;
    [SerializeField] private Cycle cycle;
    private List<int> times;        // 소화된 시간 기록
    private int timer = 20 * 60;    // 기본 20분
    private readonly int maxTime = 20 * 60;

    public static void SpawnExpObject(Vector3 targetPosition, int value)
    {
        // GameObject exp = ObjectPool.Get(
        //     PoolManager,
        //     "Exp",
        //     (parent) => Instantiate(instance.Exp, parent.transform, false)
        // );
        // exp.transform.position = targetPosition;
        // exp.GetComponent<Exp>().exp = value;
    }

    // public void AddWeapon(string weaponId)
    // {
    //     Weapon weapon = WeaponBundle.GetWeapon(weaponId);
    //     playerWeapons.Add(weapon);
    //     foreach(GameObject WeaponSlot in WeaponSlots)
    //     {
    //         if(WeaponSlot.transform.childCount == 0)
    //         {
    //             GameObject weaponSlot = Instantiate(WeaponSlotPanel, WeaponSlot.transform, false);
    //             weaponSlot.name = "Logo";
    //             weaponSlot.GetComponent<Image>().sprite = weapon.weapon.logo;
    //             if(weapon.type != "D")
    //             {
    //                 IExecuteWeapon executeWeapon = weaponSlot.AddComponent(weapon.weapon.weaponCycleScriptFile.GetClass()) as IExecuteWeapon;
    //                 executeWeapon.ExecuteWeapon(Player.instance.gameObject);
    //             }
    //             break;
    //         }
    //     }
    // }

    private void Awake()
    {
        instance = this;
        times = new(){};
    }

    private void Start()
    {
        // HeadImage.sprite = playerData.data.headImage;
        // AddWeapon(playerData.data.defaultWeapon);
        // AddWeapon("Musket");
        Player = GameObject.FindWithTag("Player");
        PoolManager = GameObject.FindWithTag("PoolManager");
        PlayerWeapons = GameObject.FindWithTag("PlayerWeapons");
        StartCoroutine(Timer());
    }

    private void Update()
    {
        // TimerText.text = TimeSpan.FromSeconds(timer).ToString(@"mm\:ss");
        // HealthBar.value = (float)playerData.health / playerData.data.stats.MaxHealth;
        // HealthText.text = playerData.health + " / " + playerData.data.stats.MaxHealth;
        // ExpBar.value = (float)playerData.exp / CalcStat.GetNeedExpFromLevel();
        // LevelText.text = "Lv " + playerData.level;
        SpawnEnemies();
    }

    private void LateUpdate()
    {
        Vector3 oldPosition = Player.transform.position;
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
                for(int i = 0; i < Random.Range(timeline.spawnCount[0], timeline.spawnCount[1]); i++)
                {
                    EnemyData enemyData = timeline.enemies[Random.Range(0, timeline.enemies.Length)];
                    GameObject enemy = EnemyManager.NewEnemy(enemyData.enemyId);
                    if(timeline.circleRadius > 0)
                    {
                        enemy.transform.position = GameUtils.MovePositionLimited(Player.transform.position + (Vector3)Random.insideUnitCircle.normalized * timeline.circleRadius, 0);
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
                                Random.Range(timeline.spawnPosition[0].x, timeline.spawnPosition[1].x),
                                Random.Range(timeline.spawnPosition[0].y, timeline.spawnPosition[1].y)
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
