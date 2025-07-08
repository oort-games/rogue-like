using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Manager<SoundManager>
{
    public const float MAX_VALUE = 100f;

    [SerializeField] AudioMixer _audioMixer;

    SoundMaster _master = new();
    Dictionary<SoundType, SoundChannel> _channels = new();
    Dictionary<string, AudioClip> _clips = new();
    
    public override void Initialize()
    {
        if (_audioMixer == null) _audioMixer = GetAudioMixer();

        _channels[SoundType.BGM] = CreateChannel(SoundType.BGM, "Master/BGM", "BGM", new GameObject("Sound BGM").AddComponent<SoundBGM>());
        _channels[SoundType.SFX] = CreateChannel(SoundType.SFX, "Master/SFX", "SFX", new GameObject("Sound SFX").AddComponent<SoundSFX>());

        _master.mixerParam = "Master";
        _master.volume = PlayerPrefs.GetInt(_master.GetVolumeSaveKey(), 100);
        _master.mute = PlayerPrefs.GetInt(_master.GetMuteSaveKey(), 0) == 1;

        foreach (var ch in _channels.Values)
        {
            ch.volume = PlayerPrefs.GetInt(ch.GetVolumeSaveKey(), 100);
            ch.mute = PlayerPrefs.GetInt(ch.GetMuteSaveKey(), 0) == 1;
        }
    }

    private void Start()
    {
        ApplyMaster();
        foreach (var type in _channels.Keys)
        {
            ApplyChannel(type);
        }
    }

    SoundChannel CreateChannel(SoundType type, string mixerPath, string mixerParam, SoundBase player)
    {
        SoundChannel channel = new()
        {
            type = type,
            mixerGroup = _audioMixer.FindMatchingGroups(mixerPath)[0],
            mixerParam = mixerParam,
            player = player,
        };

        player.transform.SetParent(transform);
        player.Initialize(channel.mixerGroup);

        return channel;
    }

    AudioMixer GetAudioMixer()
    {
        return Resources.Load<AudioMixer>("Sound/AudioMixer");
    }

    AudioClip GetClip(SoundType type, string clipName)
    {
        if (_clips.TryGetValue(clipName, out AudioClip clip))
            return clip;

        return Resources.Load<AudioClip>($"Sound/{type}/{clipName}");
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

    public float GetMasterVolume() => _master.volume;

    public float GetVolume(SoundType type) => _channels[type].volume;

    public bool GetMasterMute() => _master.mute;

    public bool GetMute(SoundType type) => _channels[type].mute;

    public void SetMasterVolume(float value)
    {
        _master.volume = Mathf.Clamp(value, 0, MAX_VALUE);
        ApplyMaster();
        SavePref(_master.GetVolumeSaveKey(), (int)_master.volume);
    }

    public void SetVolume(SoundType type, float value)
    {
        SoundChannel ch = _channels[type];
        ch.volume = Mathf.Clamp(value, 0, MAX_VALUE);
        ApplyChannel(type);
        SavePref(ch.GetVolumeSaveKey(), (int)ch.volume);
    }

    public void SetMasterMute(bool value)
    {
        _master.mute = value;
        ApplyMaster();
        SavePref(_master.GetMuteSaveKey(), _master.mute ? 1 : 0);
    }

    public void SetMute(SoundType type, bool value)
    {
        SoundChannel ch = _channels[type];
        ch.mute = value;
        ApplyChannel(type);
        SavePref(ch.GetMuteSaveKey(), ch.mute ? 1 : 0);
    }

    void ApplyMaster()
    {
        float db = _master.mute ? -80f : UiVolumeToDb(_master.volume);
        _audioMixer.SetFloat(_master.mixerParam, db);
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
