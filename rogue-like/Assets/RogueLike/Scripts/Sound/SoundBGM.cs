using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class SoundBGM : MonoBehaviour
{
    AudioSource _audioSource;

    public void Initialize(AudioMixerGroup audioMixerGroup)
    {
        _audioSource = GetComponent<AudioSource>();

        _audioSource.outputAudioMixerGroup = audioMixerGroup;
        _audioSource.loop = true;
        _audioSource.playOnAwake = false;
    }

    public void Play(AudioClip clip)
    {
        _audioSource.clip = clip;
        if (_audioSource.isPlaying)
        {
            _audioSource.Stop();
        }
        _audioSource.Play();
    }

    public void Stop()
    {
        _audioSource.Stop();
    }
}
