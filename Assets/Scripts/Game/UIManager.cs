using System.Collections;
using _20MTB.Utillity;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance {get; private set;}
    public GameObject GameOverPanel;

    [Header("게임 클리어 패널")]
    [SerializeField] private Animator GameClearPanel;
    [SerializeField] private TMP_Text KillEnemyCount;

    [Header("게임 세팅 패널")]
    [SerializeField] private GameObject GameSettingPanel;

    [Header("플레이어 상태")]
    [SerializeField] private Slider HealthSlider;
    [SerializeField] private Slider ExpSlider;
    [SerializeField] private TMP_Text HealthText;
    [SerializeField] private TMP_Text LevelText;

    private readonly string killCountMent = "{0}명의 적을 죽였습니다.";

    public void OnSettingOkBtnClick()
    {
        Game.Resume();
        GameSettingPanel.SetActive(false);
    }

    public void OnReturnBtnClick()
    {
        Time.timeScale = 1;
        AudioManager.instance.ResumeBgm();
        SceneManager.LoadScene("Main");
    }

    public void ShowGameClearPanel()
    {
        GameClearPanel.gameObject.SetActive(true);
        GameClearPanel.SetTrigger("Show");
        KillEnemyCount.text = string.Format(killCountMent, Player.playerData.killCount);
        StartCoroutine(IEHide());
    }

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if(Game.isGameOver) return;

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameSettingPanel.activeSelf)
            {
                Game.Resume();
                GameSettingPanel.SetActive(false);
            }
            else
            {
                Game.Pause();
                GameSettingPanel.SetActive(true);
            }
        }

        HealthSlider.value = (float)Player.playerData.health / Player.playerData.maxHealth;
        ExpSlider.value = (float)Player.playerData.exp / GameUtils.GetNeedExpFromLevel();
        HealthText.text = Player.playerData.health + " / " + Player.playerData.maxHealth;
        LevelText.text = "Lv " + Player.playerData.level;
    }

    IEnumerator IEHide()
    {
        yield return new WaitForSeconds(0.3f);
        Game.Pause();
    }
}
