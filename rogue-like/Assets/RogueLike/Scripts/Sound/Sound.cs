using UnityEngine;
using UnityEngine.Audio;

public enum SoundType
{
    Master,
    BGM,
    SFX
}

public class SoundChannel
{
    public SoundType type;

    public string mixerParam;

    public float volume;
    public bool mute;

    public AudioMixerGroup mixerGroup;
    public SoundPlayer player;

    public string GetVolumeSaveKey() => $"Sound.Volume.{type}";
    public string GetMuteSaveKey() => $"Sound.Mute.{type}";
}

public static class SoundExtensions
{

}