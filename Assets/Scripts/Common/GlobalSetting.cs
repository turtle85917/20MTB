using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GlobalSetting : MonoBehaviour
{
    public Character playingCharacter;
    public int selectedChannel = -1;
    public float masterVolume = 1;
    public float bgmVolume = 1;
    public float sfxVolume = 1;
    public bool showChatPanel = true;
    public static GlobalSetting instance;

    public object this[string optionName]
    {
        set {
            this.GetType().GetField(optionName).SetValue(this, value);
        }
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        showChatPanel = PlayerPrefs.GetInt("showChatPanel", 1) == 1;
    #region 볼륨 관련 설정
        masterVolume = PlayerPrefs.GetFloat("masterVolume", 1f);
        bgmVolume = PlayerPrefs.GetFloat("bgmVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 1f);
        AudioManager.instance.audioMixer.SetFloat("Master", Mathf.Log10(masterVolume) * 20f);
        AudioManager.instance.audioMixer.SetFloat("BGM", Mathf.Log10(bgmVolume) * 20f);
        AudioManager.instance.audioMixer.SetFloat("SFX", Mathf.Log10(sfxVolume) * 20f);
    #endregion
        AudioManager.instance.ChangeBgm(AudioManager.BGMClip.Main);
        Camera.main.GetComponent<UniversalAdditionalCameraData>().SetRenderer(0);
    }
}
