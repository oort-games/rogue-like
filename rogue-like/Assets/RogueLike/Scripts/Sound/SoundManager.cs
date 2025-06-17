using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Manager<SoundManager>
{
    const float MAX_VOLUME = 0f;
    const float MIN_VOLUME = -80f;

    const string BGM = "BGM";
    const string SFX = "SFX";

    const string KEY_MUTE_BGM = "SOUND_MUTE_BGM";
    const string KEY_MUTE_SFX = "SOUND_MUTE_SFX";
    const string KEY_VOLUME_BGM = "SOUND_VOLUME_BGM";
    const string KEY_VOLUME_SFX = "SOUND_VOLUME_SFX";

    public const float MAX_VALUE = 100f;

    float _volumeBGM;
    public float VolumeBGM
    {
        get { return _volumeBGM; }
        set
        {
            SetVolumeBGM(value);
        }
    }

    float _volumeSFX;
    public float VolumeSFX
    {
        get { return _volumeSFX; }
        set
        {
            SetVolumeSFX(value);
        }
    }

    bool _muteBGM;
    public bool MuteBGM
    {
        get { return _muteBGM; }
        set
        {
            SetMuteBGM(value);
        }
    }

    bool _muteSFX;
    public bool MuteSFX
    {
        get { return _muteSFX; }
        set
        {
            SetMuteSFX(value);
        }
    }

    [Header("Audio Mixer")]
    [SerializeField] AudioMixer _audioMixer;

    AudioMixerGroup _bgmAudioMixerGroup;
    AudioMixerGroup _sfxAudioMixerGroup;

    SoundBGM _soundBGM;
    SoundSFX _soundSFX;

    Dictionary<string, AudioClip> _audioClips = new();

    bool _isInitialized = false;

    public override void Initialize()
    {
        if (_audioMixer == null)
        {
            _audioMixer = GetAudioMixer();
        }
       
        _bgmAudioMixerGroup = _audioMixer.FindMatchingGroups("Master/BGM")[0];
        _sfxAudioMixerGroup = _audioMixer.FindMatchingGroups("Master/SFX")[0];

        GameObject bgmGameObject = new("Sound BGM");
        bgmGameObject.transform.SetParent(transform);
        _soundBGM = bgmGameObject.AddComponent<SoundBGM>();
        _soundBGM.Initialize(_bgmAudioMixerGroup);

        GameObject sfxGameObject = new("Sound SFX");
        sfxGameObject.transform.SetParent(transform);
        _soundSFX = sfxGameObject.AddComponent<SoundSFX>();
        _soundSFX.Initialize(_sfxAudioMixerGroup);

        MuteBGM = LoadMuteBGM();
        MuteSFX = LoadMuteSFX();
        VolumeBGM = LoadVolumeBGM();
        VolumeSFX = LoadVolumeSFX();

        _isInitialized = true;
    }

    public void PlayBGM(string clipName)
    {
        _soundBGM.Play(GetClip(SoundData.Type.BGM, clipName));
    }

    public void PlayBGM(AudioClip clip)
    {
        _soundBGM.Play(clip);
    }

    public void StopBGM()
    {
        _soundBGM.Stop();
    }

    public void PlaySFX(string clipName)
    {
        _soundSFX.Play(GetClip(SoundData.Type.SFX, clipName));
    }

    public void PlaySFX(AudioClip clip)
    {
        _soundSFX.Play(clip);
    }

    string GetClipPath(SoundData.Type type, string clipName)
    {
        return $"Sound/{type}/{clipName}";
    }

    AudioClip GetClip(SoundData.Type type, string clipName)
    {
        if (_audioClips.TryGetValue(clipName, out AudioClip clip))
        {
            return clip;
        }
        else
        {
            return Resources.Load<AudioClip>(GetClipPath(type, clipName));
        }
    }

    AudioMixer GetAudioMixer()
    {
        return Resources.Load<AudioMixer>("Sound/AudioMixer");
    }

    void SetMuteBGM(bool value)
    {
        _muteBGM = value;
        if (_muteBGM)
        {
            _audioMixer.SetFloat(BGM, MIN_VOLUME);
        }
        else
        {
            _audioMixer.SetFloat(BGM, GetVolume(_volumeBGM));
        }
        if (_isInitialized == false)
        {
            return;
        }
        SaveMuteBGM();
    }

    void SetMuteSFX(bool value)
    {
        _muteSFX = value;
        if (_muteSFX)
        {
            _audioMixer.SetFloat(SFX, MIN_VOLUME);
        }
        else
        {
            _audioMixer.SetFloat(SFX, GetVolume(_volumeSFX));
        }
        if (_isInitialized == false)
        {
            return;
        }
        SaveMuteSFX();
    }

    void SetVolumeBGM(float value)
    {
        _volumeBGM = value;
        if (_muteBGM)
        {
            return;
        }
        _audioMixer.SetFloat(BGM, GetVolume(_volumeBGM));
        if (_isInitialized == false)
        {
            return;
        }
        SaveVolumeBGM();
    }

    void SetVolumeSFX(float value)
    {
        _volumeSFX = value;
        if (_muteSFX)
        {
            return;
        }
        _audioMixer.SetFloat(SFX, GetVolume(_volumeSFX));
        if (_isInitialized == false)
        {
            return;
        }
        SaveVolumeSFX();
    }

    float GetVolume(float value)
    {
        return value / MAX_VALUE * (MAX_VOLUME - MIN_VOLUME) + MIN_VOLUME;
    }

    bool LoadMuteBGM()
    {
        return PlayerPrefs.GetInt(KEY_MUTE_BGM, 0) == 1;
    }

    bool LoadMuteSFX()
    {
        return PlayerPrefs.GetInt(KEY_MUTE_SFX, 0) == 1;
    }

    void SaveMuteBGM()
    {
        PlayerPrefs.SetInt(KEY_MUTE_BGM, _muteBGM ? 1 : 0);
        PlayerPrefs.Save();
    }

    void SaveMuteSFX()
    {
        PlayerPrefs.SetInt(KEY_MUTE_SFX, _muteSFX ? 1 : 0);
        PlayerPrefs.Save();
    }

    float LoadVolumeBGM()
    {
        return PlayerPrefs.GetInt(KEY_VOLUME_BGM, 100);
    }

    float LoadVolumeSFX()
    {
        return PlayerPrefs.GetInt(KEY_VOLUME_SFX, 100);
    }

    void SaveVolumeBGM()
    {
        PlayerPrefs.SetInt(KEY_VOLUME_BGM, (int)_volumeBGM);
        PlayerPrefs.Save();
    }

    void SaveVolumeSFX()
    {
        PlayerPrefs.SetInt(KEY_VOLUME_SFX, (int)_volumeSFX);
        PlayerPrefs.Save();
    }
}
