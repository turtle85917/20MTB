using UnityEngine;
using UnityEngine.UI;

public class SettingOption : MonoBehaviour
{
    [SerializeField] private string optionId;
    [SerializeField] private Slider slider;

    public void OnSliderValueChanged()
    {
        PlayerPrefs.SetFloat(optionId, slider.value);
        GlobalSetting.instance[optionId] = slider.value;
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
        }
    }
}
