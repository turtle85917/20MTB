using System.Collections;
using System.Collections.Generic;
using _20MTB.Utillity;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class Game : MonoBehaviour
{
    public static Vector2 maxPosition {get; private set;}
    public static GameObject PoolManager {get; private set;}
    public static CameraAgent cameraAgent {get; private set;}
    public static bool isPaused {get; private set;}
    public static bool isGameOver {get; private set;}
    public static Game instance {get; private set;}
    [Header("Game")]
    [SerializeField] private GameObject ExpPrefab;
    [SerializeField] private Cycle cycle;
    public EnemyWeapons enemyWeapons;
    [Header("UI")]
    [SerializeField] private TMP_Text TimerText;
    private List<int> times;                // 소환된 시간 기록
    private int time = 0;            // 기본 20분
    private readonly int maxTime = 20 * 60; // 최대 시간 20분

    public static void Pause()
    {
        Time.timeScale = 0;
        isPaused = true;
    }

    public static void Resume()
    {
        Time.timeScale = 1;
        isPaused = false;
    }

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
        cameraAgent = Camera.main.GetComponent<CameraAgent>();
    }

    private void Start()
    {
        PoolManager = GameObject.FindWithTag("PoolManager");
        maxPosition = new Vector2(Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize);
        StartCoroutine(CheckTime());
    }

    private void LateUpdate()
    {
        if(Player.playerData.health <= 0 && !isGameOver)
        {
            isGameOver = true;
            Camera.main.GetComponent<Animation>().Play("Camera_ZoomIn");
            Player.@object.GetComponent<Player>().enabled = false;
            Player.@object.GetComponent<SortingGroup>().enabled = false;
            Player.@object.GetComponent<Affecter>().Reset();
            StopAllCoroutines();
            WeaponBundle.instance.StopAllCoroutines();
        }
    }

    private void SpawnEnemies()
    {
        foreach(CycleTimeline timeline in cycle.cycleTimelines)
        {
            if(time == timeline.time && !times.Contains(timeline.time))
            {
                times.Add(timeline.time);
                for(int i = 0; i < Random.Range(timeline.spawnCount.x, timeline.spawnCount.y); i++)
                {
                    EnemyData enemyData = timeline.enemies[Random.Range(0, timeline.enemies.Length)];
                    GameObject enemy = EnemyManager.NewEnemy(enemyData.enemyId);
                    if(timeline.circleRadius > 0)
                    {
                        enemy.transform.position = GameUtils.MovePositionLimited(Player.@object.transform.position + (Vector3)Random.insideUnitCircle.normalized * timeline.circleRadius);
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

    private IEnumerator CheckTime()
    {
        while(time < maxTime)
        {
            SpawnEnemies();
            if(time > 0 && time % 15 == 0)
            {
                enemyWeapons.PickupWeapons();
            }
            yield return new WaitForSeconds(1f);
            time += 1;
            TimerText.text = System.TimeSpan.FromSeconds(maxTime - time).ToString(@"mm\:ss");
        }
    }
}
