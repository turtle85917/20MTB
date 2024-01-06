using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource bgmAudioSource;
    [SerializeField] private AudioData audioData;

    private int channelCount = 15;

    public enum BGMClip
    {
        Main,
        Game
    }

    public enum SFXClip
    {

    }

    public void ChangeBgm(BGMClip BGM)
    {
        bgmAudioSource.Stop();
        AudioData.Clip data = audioData.bgmClips[(int)BGM];
        bgmAudioSource.clip = data.clip;
        bgmAudioSource.volume = data.volume;
        bgmAudioSource.Play();
    }

    public void PlaySound()
    {
        
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
