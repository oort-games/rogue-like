using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class SoundSFX : MonoBehaviour
{
    AudioSource _audioSource;

    public void Initialize(AudioMixerGroup audioMixerGroup)
    {
        _audioSource = GetComponent<AudioSource>();

        _audioSource.outputAudioMixerGroup = audioMixerGroup;
        _audioSource.loop = false;
        _audioSource.playOnAwake = false;
    }

    public void Play(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }
}
