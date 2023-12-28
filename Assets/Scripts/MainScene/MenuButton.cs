using UnityEngine;

public class MenuButton : MonoBehaviour
{
    public void OpenPanel(GameObject Panel)
    {
        Panel.SetActive(true);
        Panel.GetComponent<Animation>()?.Play("Panel_Show2");
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
