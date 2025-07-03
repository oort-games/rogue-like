using UnityEngine;
using UnityEngine.Audio;

public enum SoundType
{
    BGM,
    SFX
}

public class SoundChannel
{
    public SoundType type;
    
    public AudioMixerGroup mixerGroup;
    public string mixerParam;

    public float volume;
    public bool mute;

    public SoundBase player;

    public string GetVolumeSaveKey()
    {
        return $"Sound_Volume_{type}";
    }

    public string GetVolumeMuteKey()
    {
        return $"Sound_Mute_{type}";
    }
}
