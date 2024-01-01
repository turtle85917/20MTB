using UnityEngine;

public class GlobalSetting : MonoBehaviour
{
    public Character playingCharacter;
    public int selectedChannel = -1;
    public int masterVolume = 1;
    public int bgmVolume = 1;
    public int sfxVolume = 1;
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
}
