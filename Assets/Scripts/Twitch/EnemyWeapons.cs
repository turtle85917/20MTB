using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _20MTB.Utillity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyWeapons : MonoBehaviour
{
    [Header("Conent 2 관련")]
    public Image logo;
    public TMP_Text upgradeText;
    public GameObject[] contents;
    private VoteItem[] voteItems;
    public List<Weapon> weapons;
    [HideInInspector] public bool isInitial = true;
    private float startedAt;
    private Dictionary<string, List<Chat>> spawners;
    private int participants;
    private bool isVoting;
    private new Animation animation;

    private readonly int MAX_PARTICIPANT_COUNT = 30;
    private readonly string UPGRADE_TEXT_TEMPLATE = "{0} <size=21>무기 업그레이드</size>\n<size=20>(<color=#BDBDBD>{1}</color> 님의 적)</size>";

    public void ShowContent(int index)
    {
        foreach(GameObject content in contents) content.SetActive(false);
        contents[index].SetActive(true);
    }

    public void PlayActiveContentAnimated()
    {
        foreach(GameObject content in contents)
        {
            if(content.activeSelf)
            {
                content.GetComponent<Animation>().Play("Content_Show");
            }
        }
    }

    public void PickupWeapons()
    {
        if(isVoting) return;
        startedAt = Time.time;
        isVoting = true;
        ShowContent(0);
        animation.Play("Panel_Show");
        Weapon[] useableWeapons = WeaponBundle.GetWeapons(item => item.type == "N");
        foreach(VoteItem voteItem in voteItems)
        {
            Weapon weapon;
            do weapon = useableWeapons[Random.Range(0, useableWeapons.Length)];
            while(weapons.Exists(item => item.weapon.weaponId == weapon.weapon.weaponId));
            weapons.Add(weapon);
            spawners.Add(weapon.weapon.weaponId, new List<Chat>(){});
            voteItem.UpdateWeaponPanel(weapon);
        }
        if(isInitial) isInitial = false;
    }

    public GameObject SpawnEnemy(Chat twitchUser, string weaponId)
    {
        Weapon weapon = WeaponBundle.GetWeaponByName(weaponId);
        if(weapon == null) return null;
        if(weapons.Count == 0) return null;
        if(spawners.Values.ToList().Exists(item => item.Exists(t => t.userId == twitchUser.userId))) return null;
        if(spawners.ContainsKey(weapon.weapon.weaponId))
        {
            spawners.TryGetValue(weapon.weapon.weaponId, out List<Chat> users);
            users.Add(twitchUser);

            participants++;

            int index = spawners.Keys.ToList().IndexOf(weapon.weapon.weaponId);
            voteItems[index].UpdateSlider(users.Count / participants);
            GameObject enemy = EnemyManager.NewEnemy("Panzee", twitchUser.userId);
            WeaponBundle.AddWeaponToTarget(enemy, weapon.weapon.weaponId);
            enemy.transform.position = GameUtils.MovePositionLimited(Player.@object.transform.position + (Vector3)Random.insideUnitCircle.normalized * 10);
            return enemy;
        }
        return null;
    }

    private void Awake()
    {
        weapons = new List<Weapon>();
        spawners = new Dictionary<string, List<Chat>>();
        voteItems = GetComponentsInChildren<VoteItem>();
        animation = GetComponent<Animation>();
    }

    private void Update()
    {
        if((Time.time - startedAt > 50f || participants == MAX_PARTICIPANT_COUNT) && isVoting)
        {
            isVoting = false;
            UpgradeRandomEnemyWeapon();
        }
    }

    private void UpgradeRandomEnemyWeapon() 
    {
        if(participants == 0)
        {
            animation.Play("Panel_Hide");
            return;
        }
        string weaponId;
        // NOTE: 가장 많이 쏠린 순서로 정렬 및 1회 이상 모인 것만 불러오기
        Dictionary<string, Chat[]> orderdSpawners = spawners.OrderBy(item => item.Value.Count).ToDictionary(x => x.Key, x => x.Value.ToArray());
        Dictionary<string, Chat[]> filterdSpawners = spawners.Where(item => item.Value.Count > 0).ToDictionary(x => x.Key, x => x.Value.ToArray());
        int[] participantValues = spawners.Select(item => item.Value.Count).ToArray();
        // NOTE: 편차 구하여 편차가 1 이하일 경우 (값이 일정한 비율)
        float average = participantValues.Sum() / participantValues.Length;
        float variance = participantValues.Aggregate(0f, (acc, curr) => acc + Mathf.Pow(curr - average, 2)) / participantValues.Length;
        Chat twitchUser;
        if(variance <= 1f)
        {
            weaponId = filterdSpawners.Keys.ToList()[Random.Range(0, filterdSpawners.Keys.Count)];
            Chat[] twitchUsers = filterdSpawners.GetValueOrDefault(weaponId).ToArray();
            twitchUser = twitchUsers[Random.Range(0, twitchUsers.Length)];
        }
        else
        {
            KeyValuePair<string, Chat[]> maximumSpawner = orderdSpawners.First();
            weaponId = maximumSpawner.Key;
            twitchUser = maximumSpawner.Value[Random.Range(0, maximumSpawner.Value.Length)];
        }
        // NOTE: 적의 무기 업그레이드
        EnemyPool target = EnemyManager.GetEnemy(twitchUser.userId);
        if(target == null || target.weapon == null)
        {
            animation.Play("Panel_Hide");
            return;
        }
        Weapon weapon = WeaponBundle.GetWeapon(weaponId);
        // NOTE: 무기가 만렙일 경우, 컷
        WeaponBundle.UpgradeTargetsWeapon(target.target, weaponId);
        logo.sprite = weapon.weapon.logo;
        upgradeText.text = string.Format(UPGRADE_TEXT_TEMPLATE, weapon.name, twitchUser.userName);
        // NOTE: 데이터 초기화
        weapons = new List<Weapon>();
        spawners = new Dictionary<string, List<Chat>>();
        participants = 0;
        // NOTE: 판넬 보이기
        ShowContent(1);
        PlayActiveContentAnimated();
        // NOTE: 대기 후, 판넬 숨기기
        StartCoroutine(HidePanel());
    }

    private IEnumerator HidePanel()
    {
        yield return new WaitForSecondsRealtime(3f);
        animation.Play("Panel_Hide");
    }
}
