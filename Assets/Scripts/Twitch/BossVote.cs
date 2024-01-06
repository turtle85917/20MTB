using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossVote : MonoBehaviour
{
    [SerializeField] private EnemyData[] bosses;

    [Header("보스 체력 관련")]
    [SerializeField] private TMP_Text BossName;
    [SerializeField] private Slider bossHealth;

    private bool isBossVoting;
    private Dictionary<string, List<Chat>> participants;

    private float startedAt;

    private VoteItem[] voteItems;
    private new Animation animation;

    private EnemyData bossData;
    private EnemyPool bossPool;

    public void ShowContent()
    {}

    public void StartVoting()
    {
        if(isBossVoting) return;
        animation.Play("Panel_Show");
        participants = new Dictionary<string, List<Chat>>(){};
        foreach(VoteItem voteItem in voteItems)
        {
            EnemyData boss;
            do boss = bosses[Random.Range(0, bosses.Length)];
            while(participants.Keys.Contains(boss.enemyId));
            participants.Add(boss.enemyId, new List<Chat>(){});

            voteItem.UpdatePanel(boss.bossSprite, boss.bossName);
        }
        isBossVoting = true;
        startedAt = Time.time;
    }
    public void SpawnBoss()
    {
        BossName.text = "BOSS ! " + bossData.bossName;
        bossHealth.value = 1f;

        // NOTE: 다 죽임
        EnemyPool[] enemyPools = EnemyManager.GetEnemies();
        foreach(EnemyPool enemyPool in enemyPools)
        {
            Destroy(enemyPool.target);
            EnemyManager.RemoveEnemy(enemyPool);
        }

        GameObject bossObject = EnemyManager.NewBoss(bossData);
        bossObject.transform.position = Player.@object.transform.position + (Vector3)Random.insideUnitCircle * 13;

        bossPool = EnemyManager.GetEnemy(bossObject);
    }

    public void OnVote(Chat chat, int bossIndex)
    {
        if(!isBossVoting) return;
        if(participants.Values.ToList().Exists(item => item.Exists(t => t.userId == chat.userId))) return;
        participants.ElementAt(bossIndex).Value.Add(chat);
        voteItems[bossIndex].UpdateSlider(participants.ElementAt(bossIndex).Value.Count / participants.Values.Sum(item => item.Count));
    }

    private void Awake()
    {
        voteItems = GetComponentsInChildren<VoteItem>();
        animation = GetComponent<Animation>();
    }

    private void Update()
    {
        if(Time.time - startedAt >= 30f && isBossVoting)
        {
            animation.Play("Panel_Hide");
            isBossVoting = false;
            GetVoteResult();
        }
        if(bossPool != null)
        {
            bossHealth.value = bossPool.health / bossPool.data.stats.MaxHealth;
            if(bossPool.health <= 0) bossPool = null;
        }
    }

    private void GetVoteResult()
    {
        string bossId;
        // NOTE: 편차 구하기
        int[] participantValues = participants.Values.Select(item => item.Count).ToArray();
        float average = participantValues.Sum() / participantValues.Length;
        float variance = participantValues.Aggregate(0f, (acc, curr) => acc + Mathf.Pow(curr - average, 2)) / participantValues.Length;
        // NOTE: 편차가 1 이하일 경우, 투표율이 고르다 판정
        if(variance <= 1f) bossId = participants.Keys.ToList()[Random.Range(0, participants.Keys.Count)];
        else bossId = participants.OrderBy(item => item.Value.Count).First().Key;
        bossData = bosses.FirstOrDefault(item => item.enemyId == bossId);
    }
}
