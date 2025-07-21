using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Manager<SoundManager>
{
    public const float MAX_VALUE = 100f;

    [SerializeField] AudioMixer _audioMixer;

    Dictionary<SoundType, SoundChannel> _channels = new();
    Dictionary<string, AudioClip> _clips = new();

    public float GetVolume(SoundType type) => _channels[type].volume;
    public bool GetMute(SoundType type) => _channels[type].mute;

    public override void Initialize()
    {
        _channels[SoundType.Master] = CreateChannel(SoundType.Master, "Master", "Master", new GameObject("Sound Master Player").AddComponent<SoundMasterPlayer>());
        _channels[SoundType.BGM] = CreateChannel(SoundType.BGM, "BGM", "Master/BGM", new GameObject("Sound BGM Player").AddComponent<SoundBGMPlayer>());
        _channels[SoundType.SFX] = CreateChannel(SoundType.SFX, "SFX", "Master/SFX", new GameObject("Sound SFX Player").AddComponent<SoundSFXPlayer>());

        foreach (var ch in _channels.Values)
        {
            ch.volume = PlayerPrefs.GetInt(ch.GetVolumeSaveKey(), 100);
            ch.mute = PlayerPrefs.GetInt(ch.GetMuteSaveKey(), 0) == 1;
        }
    }

    void Start()
    {
        foreach (var type in _channels.Keys)
        {
            ApplyChannel(type);
        }
    }

    SoundChannel CreateChannel(SoundType type, string mixerParam, string mixerPath, SoundPlayer player)
    {
        SoundChannel channel = new()
        {
            type = type,
            mixerParam = mixerParam,
            mixerGroup = _audioMixer.FindMatchingGroups(mixerPath)[0],
            player = player,
        };

        player.transform.SetParent(transform);
        player.Initialize(channel.mixerGroup);

        return channel;
    }

    AudioClip GetClip(SoundType type, string clipName)
    {
        if (_clips.TryGetValue(clipName, out AudioClip clip))
            return clip;

        return Resources.Load<AudioClip>($"Sounds/{type}/{clipName}");
    }

    public void Play(SoundType type, string clipName)
    {
        _channels[type].player.Play(GetClip(type, clipName));
    }

    public void Play(SoundType type, AudioClip clip)
    {
        _channels[type].player.Play(clip);
    }

    public void Stop(SoundType type)
    {
        _channels[type].player.Stop();
    }

    public void SetVolume(SoundType type, float value)
    {
        SoundChannel ch = _channels[type];
        ch.volume = Mathf.Clamp(value, 0, MAX_VALUE);
        ApplyChannel(type);
        SavePref(ch.GetVolumeSaveKey(), (int)ch.volume);
    }

    public void SetMute(SoundType type, bool value)
    {
        SoundChannel ch = _channels[type];
        ch.mute = value;
        ApplyChannel(type);
        SavePref(ch.GetMuteSaveKey(), ch.mute ? 1 : 0);
    }

    void ApplyChannel(SoundType type)
    {
        SoundChannel ch = _channels[type];
        float db = ch.mute ? -80f : UiVolumeToDb(ch.volume);
        _audioMixer.SetFloat(ch.mixerParam, db);
    }

    float UiVolumeToDb(float uiValue)
    {
        float linear = Mathf.Clamp01(uiValue / MAX_VALUE);
        return Mathf.Log10(Mathf.Max(linear, 0.0001f)) * 20f;
    }

    void SavePref(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
    }
}
