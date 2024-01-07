using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public static AudioManager instance;

    [SerializeField] private AudioSource bgmAudioSource;
    [SerializeField] private AudioData audioData;

    [Header("오디오 믹서 그룹")]
    [SerializeField] private AudioMixerGroup sfxGroup;

    private int channelCount = 15;
    private List<AudioSource> sfxSources;

    public enum BGMClip
    {
        Main
    }
    public enum SFXClip
    {
        WeaponOmurice,
        WeaponMuayThai
    }

    public void ChangeBgm(BGMClip BGM)
    {
        bgmAudioSource.Stop();
        AudioData.Clip data = audioData.bgmClips[(int)BGM];
        bgmAudioSource.clip = data.clip;
        bgmAudioSource.volume = data.volume;
        bgmAudioSource.Play();
    }
    public void PauseBgm()
    {
        bgmAudioSource.Pause();
    }
    public void ResumeBgm()
    {
        bgmAudioSource.time = 0;
        bgmAudioSource.Play();
    }

    public void PlaySound(SFXClip SFX)
    {
        bool isPlaying = false;
        AudioData.Clip data = audioData.sfxClips[(int)SFX];
        for(int i = 0; i < channelCount; i++)
        {
            if(!sfxSources[i].isPlaying)
            {
                sfxSources[i].clip = data.clip;
                sfxSources[i].volume = data.volume;
                sfxSources[i].Play();
                isPlaying = true;
                break;
            }
        }
        if(!isPlaying)
        {
            AudioSource audioSource = NewAudioSource();
            audioSource.clip = data.clip;
            audioSource.volume = data.volume;
            audioSource.Play();
            channelCount++;
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
        sfxSources = new List<AudioSource>();
        for(int i = 0; i < channelCount; i++)
        {
            NewAudioSource();
        }
    }

    private AudioSource NewAudioSource()
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = false;
        audioSource.playOnAwake = false;
        audioSource.outputAudioMixerGroup = sfxGroup;
        sfxSources.Add(audioSource);
        return audioSource;
    }
}
