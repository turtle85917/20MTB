using System;
using UnityEngine;

[CreateAssetMenu(menuName = "오디오 데이터")]
public class AudioData : ScriptableObject
{
    public Clip[] bgmClips;
    public Clip[] sfxClips;

    [Serializable]
    public class Clip
    {
        public AudioClip clip;
        [Range(0, 1)] public float volume = 1f;
        public bool isLoop;
    }
}
