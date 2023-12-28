using UnityEngine;

public class GlobalSetting : MonoBehaviour
{
    public Character playingCharacter;
    public static GlobalSetting instance;

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
