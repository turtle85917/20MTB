using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _20MTB.Utillity;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Game : MonoBehaviour
{
    public static Vector2 maxPosition {get; private set;}
    public static GameObject PoolManager {get; private set;}
    public static CameraAgent cameraAgent {get; private set;}
    public static bool isPaused {get; private set;}
    public static bool isGameOver {get; private set;}
    public static Game instance {get; private set;}

    [Header("Game")]
    [SerializeField] private Cycle cycle;
    public Drops drops;
    public BossVote bossVote;
    public EnemyWeapons usableWeaponsPanel;
    public DonatedBox donatedBoxPanel;
    [Header("UI")]
    [SerializeField] private TMP_Text TimerText;

    private UniversalAdditionalCameraData cameraData;

    private int stage;
    private string lastEnemyId;

    private List<int> times;
    private int time = 0;
    private float spawnDelay = 1f;

    private readonly float decreaseSpawnDelay = 0.9f;
    private readonly int maxTime = 20 * 60; // 최대 시간 20분
    private readonly Vector2[] directions = new Vector2[]{
        Vector2.up,
        Vector2.down,
        Vector2.right,
        Vector2.left
    };

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

    private void Awake()
    {
        instance = this;
        times = new List<int>(){};
        cameraAgent = Camera.main.GetComponent<CameraAgent>();
        cameraData = Camera.main.GetComponent<UniversalAdditionalCameraData>();
    }

    private void Start()
    {
        PoolManager = GameObject.FindWithTag("PoolManager");
        maxPosition = new Vector2(Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize);
        Resume();
        isGameOver = false;
        cameraData.SetRenderer(1);
        StartCoroutine(CheckTime());
        StartCoroutine(FreeSpawnEnemy());
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
            if(time > 0 && time % 10 == 0) drops.StartDrops();
            if(time > 0 && time % 15 == 0) usableWeaponsPanel.PickupWeapons();
            if(time > 0 && time % 120 == 0) spawnDelay *= decreaseSpawnDelay;
            if(time == 19 * 60) bossVote.StartVoting();
            SpawnEnemies();
            yield return new WaitForSeconds(1f);
            time += 1;
            TimerText.text = System.TimeSpan.FromSeconds(maxTime - time).ToString(@"mm\:ss");
        }
        bossVote.SpawnBoss();
    }

    private IEnumerator FreeSpawnEnemy()
    {
        while(true)
        {
            yield return new WaitForSeconds(spawnDelay);
            if(time > 5)
            {
                Vector2[] spawnableDirections = directions.Where(CheckSpawnablePosition).ToArray();
                Vector2 direction = spawnableDirections[Random.Range(0, spawnableDirections.Length)];
                string enemyId = GetRandomEnemy();
                stage++;
                if(lastEnemyId != null && stage < 5) enemyId = lastEnemyId;
                else lastEnemyId = enemyId;
                GameObject enemy = EnemyManager.NewEnemy(enemyId);
                enemy.transform.position = (Vector2)Player.@object.transform.position + direction * 10f + Random.insideUnitCircle * 6;

                bool CheckSpawnablePosition(Vector2 direction)
                {
                    Vector2 fixedPosition = (Vector2)Player.@object.transform.position + direction * 10f;
                    return GameUtils.maxPosition.x >= fixedPosition.x && GameUtils.maxPosition.y >= fixedPosition.y && GameUtils.minPosition.x <= fixedPosition.x && GameUtils.minPosition.y <= fixedPosition.y;
                }

                string GetRandomEnemy()
                {
                    List<string> spawnableEnemies = new List<string>(){"Bat"};
                    if(time >= 20) spawnableEnemies.Add("Pigeon");
                    if(time >= 40) spawnableEnemies.Add("Panzee");
                    if(time >= 100) spawnableEnemies.Add("Leaf");
                    if(time >= 600) spawnableEnemies.Add("Fox");
                    return spawnableEnemies[Random.Range(0, spawnableEnemies.Count)];
                }
            }
        }
    }
}
