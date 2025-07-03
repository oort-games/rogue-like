using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Manager<SoundManager>
{
    public const float MAX_VALUE = 100f;

    const float MAX_DB = 0f;
    const float MIN_DB = -80f;

    [SerializeField] AudioMixer _audioMixer;

    Dictionary<SoundType, SoundChannel> _channels = new();
    Dictionary<string, AudioClip> _clips = new();
    
    public override void Initialize()
    {
        if (_audioMixer == null) _audioMixer = GetAudioMixer();
 
        _channels[SoundType.BGM] = CreateChannel(SoundType.BGM, "Master/BGM", "BGM", new GameObject("Sound BGM").AddComponent<SoundBGM>());
        _channels[SoundType.SFX] = CreateChannel(SoundType.SFX, "Master/SFX", "SFX", new GameObject("Sound SFX").AddComponent<SoundSFX>());

        foreach(var (type, ch) in _channels)
        {
            ch.volume = PlayerPrefs.GetInt(ch.GetVolumeSaveKey(), 100);
            ch.mute = PlayerPrefs.GetInt(ch.GetVolumeMuteKey(), 0) == 1;
            ApplyVolume(type);
            ApplyMute(type);
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

    public float GetVolume(SoundType type)
    {
        return _channels[type].volume;
    }

    public bool GetMute(SoundType type)
    {
        return _channels[type].mute;
    }

    public void SetVolume(SoundType type, float value)
    {
        SoundChannel ch = _channels[type];
        ch.volume = Mathf.Clamp(value, 0, MAX_VALUE);
        ApplyVolume(type);
        SavePref(ch.GetVolumeSaveKey(), (int)ch.volume);
    }

    public void SetMute(SoundType type, bool value)
    {
        SoundChannel ch = _channels[type];
        ch.mute = value;
        ApplyMute(type);
        SavePref(ch.GetVolumeMuteKey(), ch.mute ? 1 : 0);
    }

    void ApplyVolume(SoundType type)
    {
        SoundChannel ch = _channels[type];
        if (ch.mute) return;
        _audioMixer.SetFloat(ch.mixerParam, UiVolumeToDb(ch.volume));
    }

    void ApplyMute(SoundType type)
    {
        SoundChannel ch = _channels[type];
        _audioMixer.SetFloat(ch.mixerParam, ch.mute ? MIN_DB : UiVolumeToDb(ch.volume));
    }

    float UiVolumeToDb(float uiValue)
    {
        return uiValue / MAX_VALUE * (MAX_DB - MIN_DB) + MIN_DB;
    }

    void SavePref(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
    }
}
