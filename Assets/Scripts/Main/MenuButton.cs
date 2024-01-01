using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private Alert alert;

    public void OnOpeningEnd()
    {
        SceneManager.LoadScene("SelectCharacter");
        PlayerPrefs.SetInt("showedOpening", 1);
    }

    public void OnStartBtnClick()
    {
        if(GlobalSetting.instance.selectedChannel == -1)
        {
            alert.ShowAlert("채널을 먼저 선택해주세요.");
            return;
        }
        SceneManager.LoadScene(
            PlayerPrefs.GetInt("showedOpening") == 0
            ? "Opening"
            : "SelectCharacter"
        );
    }

    public void OpenPanel(GameObject Panel)
    {
        Panel.SetActive(true);
        Panel.GetComponent<Animation>().Play("Panel_Show2");
    }

    public void PanelCancel(GameObject Panel)
    {
        Panel.SetActive(false);
    }

    public void Quit()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}
