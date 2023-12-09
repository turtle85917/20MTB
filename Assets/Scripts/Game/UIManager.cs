using _20MTB.Utillity;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance {get; private set;}
    public GameObject GameOverPanel;
    [SerializeField] private Slider HealthSlider;
    [SerializeField] private Slider ExpSlider;
    [SerializeField] private TMP_Text HealthText;
    [SerializeField] private TMP_Text LevelText;

    public void OnReturnBtnClick()
    {
        SceneManager.LoadScene("Main");
    }

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if(Game.isGameOver) return;
        HealthSlider.value = (float)Player.playerData.health / Player.playerData.data.stats.MaxHealth;
        ExpSlider.value = (float)Player.playerData.exp / GameUtils.GetNeedExpFromLevel();
        HealthText.text = Player.playerData.health + " / " + Player.playerData.data.stats.MaxHealth;
        LevelText.text = "Lv " + Player.playerData.level;
    }
}
