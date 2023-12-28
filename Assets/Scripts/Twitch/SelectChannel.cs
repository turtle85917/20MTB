using UnityEngine;
using UnityEngine.UI;

public class SelectChannel : MonoBehaviour
{
    [SerializeField] private Button[] buttons;
    private readonly Color selectedColor = new Color(72f / 255f, 238 / 255f, 250 / 255f);

    private void Start()
    {
        ResetButtonImageColor();
        buttons[PlayerPrefs.GetInt("SelectedChannel")].gameObject.GetComponent<Image>().color = selectedColor;
        for(int i = 0; i < buttons.Length; i++)
        {
            buttons[i].onClick.RemoveAllListeners();
            int index = i;
            buttons[i].onClick.AddListener(() => OnChannelClick(index));
        }
    }

    private void OnChannelClick(int index)
    {
        PlayerPrefs.SetInt("SelectedChannel", index);
        ResetButtonImageColor();
        buttons[index].gameObject.GetComponent<Image>().color = selectedColor;
    }

    private void ResetButtonImageColor()
    {
        foreach(Button button in buttons)
            button.gameObject.GetComponent<Image>().color = Color.white;
    }
}
