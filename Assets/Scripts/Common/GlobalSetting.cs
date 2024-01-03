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
        Camera.main.GetComponent<UniversalAdditionalCameraData>().SetRenderer(0);
    }
}
