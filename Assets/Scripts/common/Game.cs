using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public Character character;
    public PlayerData playerData;
    public static Game instance {get; private set;}
    [SerializeField] private PlayerData[] players;
    [SerializeField] private EnemyData[] enemies;
    [SerializeField] private Image HeadImage;
    [SerializeField] private TMP_Text TimerText;
    private int timer = 20 * 60; // 기본 20분
    private readonly int maxTime = 20 * 60; // 2분

    private void Awake()
    {
        instance = this;
        playerData = players[(int)character];
    }

    private void Start()
    {
        HeadImage.sprite = playerData.headImage;
        StartCoroutine(Timer());
    }

    private void Update()
    {
        TimerText.text = TimeSpan.FromSeconds(timer).ToString(@"mm\:ss");
    }

    IEnumerator Timer()
    {
        while(timer > 0)
        {
            yield return new WaitForSeconds(1f);
            timer -= 1;
        }
    }
}
