using System.Collections;
using System.Collections.Generic;
using _20MTB.Utillity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public static GameObject PoolManager;
    public static Game instance {get; private set;}
    [Header("Game")]
    [SerializeField] private GameObject ExpPrefab;
    [SerializeField] private Cycle cycle;
    [Header("UI - Player Status")]
    [SerializeField] private Slider HealthSlider;
    [SerializeField] private Slider ExpSlider;
    [SerializeField] private TMP_Text HealthText;
    [SerializeField] private TMP_Text LevelText;
    [Header("UI")]
    [SerializeField] private TMP_Text TimerText;
    private List<int> times;                // 소화된 시간 기록
    private int timer = 20 * 60;            // 기본 20분
    private readonly int maxTime = 20 * 60; // 최대 시간 20분

    public static void SpawnExpObject(Vector3 targetPosition, int value)
    {
        GameObject exp = ObjectPool.Get(PoolManager, "Exp", instance.ExpPrefab);
        exp.transform.position = targetPosition;
        exp.GetComponent<Exp>().exp = value;
    }

    private void Awake()
    {
        instance = this;
        times = new List<int>(){};
    }

    private void Start()
    {
        PoolManager = GameObject.FindWithTag("PoolManager");
        StartCoroutine(Timer());
    }

    private void Update()
    {
        TimerText.text = System.TimeSpan.FromSeconds(timer).ToString(@"mm\:ss");
        HealthSlider.value = (float)Player.playerData.health / Player.playerData.data.stats.MaxHealth;
        ExpSlider.value = (float)Player.playerData.exp / GameUtils.GetNeedExpFromLevel();
        HealthText.text = Player.playerData.health + " / " + Player.playerData.data.stats.MaxHealth;
        LevelText.text = "Lv " + Player.playerData.level;
        SpawnEnemies();
    }

    private void LateUpdate()
    {
        Camera.main.transform.position = Player.@object.transform.position + Vector3.back * 10;
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
                        enemy.transform.position = GameUtils.MovePositionLimited(Player.@object.transform.position + (Vector3)Random.insideUnitCircle.normalized * timeline.circleRadius, 0);
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
