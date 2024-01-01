using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingOption : MonoBehaviour
{
    [SerializeField] private string optionId;

    [Header("슬라이더로 값 조절")]
    [SerializeField] private Slider slider;

    [Header("화살표로 값 조절")]
    [SerializeField] private GameObject prevArrow;
    [SerializeField] private GameObject nextArrow;
    [SerializeField] private TMP_Text valueName;
    [SerializeField] private Connect connect;

    public void OnSliderValueChanged()
    {
        PlayerPrefs.SetFloat(optionId, slider.value);
        GlobalSetting.instance[optionId] = slider.value;
    }

    public void OnArrowClick(int isNext)
    {
        PlayerPrefs.SetInt(optionId, isNext);
        ChangeArrowOption(true);
    }

    private void Start()
    {
        switch(optionId)
        {
            case "masterVolume":
            case "bgmVolume":
            case "sfxVolume":
                slider.value = PlayerPrefs.GetFloat(optionId, 1f);
                GlobalSetting.instance[optionId] = slider.value;
                break;
            case "showChatPanel":
                ChangeArrowOption(false);
                break;
        }
    }

    private void ChangeArrowOption(bool update)
    {
        GlobalSetting.instance.showChatPanel = PlayerPrefs.GetInt(optionId, 1) == 1;
        if(update && !GlobalSetting.instance.showChatPanel) connect.RemoveAllChats();
        valueName.text = GlobalSetting.instance.showChatPanel ? "켜짐" : "꺼짐";
        prevArrow.SetActive(GlobalSetting.instance.showChatPanel);
        nextArrow.SetActive(!prevArrow.activeSelf);
    }
}
