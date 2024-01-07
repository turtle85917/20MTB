using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Drops : MonoBehaviour
{
    public Animation ParachutePanel;
    public Image Icon;
    public TMP_Text Name;
    [HideInInspector] public bool isInitial = true;

    [HideInInspector] public Chat dropsActivator;
    [SerializeField] private TMP_Text DropsActivator;
    [SerializeField] private Slider slider;
    private new Animation animation;

    [SerializeField] private GameObject Parachute;

    private List<Chat> participants;
    private bool isDropsActive;

    private int dropCount;
    private float startedAt;

    private readonly int MAX_PARTICIPANT_COUNT = 40;

    public void ShowContent()
    {}

    public void JoinDrops(Chat chat)
    {
        if(!isDropsActive) return;
        if(participants.Exists(item => item.userId == chat.userId)) return;
        participants.Add(chat);
        slider.value = participants.Count / (float)MAX_PARTICIPANT_COUNT;
    }

    public void SpawnDrop(string giftType, string weaponName)
    {
        GameObject parachute = ObjectPool.Get(Game.PoolManager, "Parachute", Parachute);
        float maxY = Game.maxPosition.y + Camera.main.transform.position.y;
        parachute.transform.position = new Vector3(Player.@object.transform.position.x + Random.Range(-3, 3), maxY);
        Parachute script = parachute.GetComponent<Parachute>();
        script.giftType = giftType;
        script.weaponName = weaponName;
        dropCount++;
        if(giftType == "weapon") dropCount = 3;
    }

    public void StartDrops()
    {
        if(dropsActivator != null) return;
        if(isDropsActive) return;
        ResetDrops();
        animation.Play("Panel_Show");
        startedAt = Time.time;
        if(isInitial) isInitial = false;
    }

    public void HideParachutePanel()
    {
        StartCoroutine(IEHide());
    }

    private void Awake()
    {
        animation = GetComponent<Animation>();
    }

    private void Update()
    {
        if(isDropsActive && (Time.time - startedAt > 40f || participants.Count == MAX_PARTICIPANT_COUNT))
        {
            isDropsActive = false;
            GetDropsActivator();
        }
        if(dropsActivator != null && dropCount == 3)
        {
            ResetDrops();
        }
    }

    private void ResetDrops()
    {
        participants = new List<Chat>(){};
        dropsActivator = null;
        dropCount = 0;
        isDropsActive = true;
        DropsActivator.gameObject.SetActive(false);
        slider.value = 0f;
    }

    private void GetDropsActivator()
    {
        animation.Play("Panel_Hide");
        if(participants.Count == 0) return;
        Chat activator = participants[Random.Range(0, participants.Count)];
        dropsActivator = activator;
        DropsActivator.text = $"드롭스 보상자 : {activator.userName}";
        DropsActivator.gameObject.SetActive(true);
    }

    public IEnumerator IEHide()
    {
        yield return new WaitForSeconds(1f);
        ParachutePanel.gameObject.SetActive(false);
    }
}
