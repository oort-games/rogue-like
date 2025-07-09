using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public abstract class SoundPlayer : MonoBehaviour
{
    protected AudioSource _source;

    public virtual void Initialize(AudioMixerGroup mixerGroup)
    {
        _source = GetComponent<AudioSource>();

        _source.outputAudioMixerGroup = mixerGroup;
        _source.playOnAwake = false;
    }

    public virtual void Play(AudioClip clip)
    {
        if (clip == null) return;
        _source.clip = clip;
        _source.Play();
    }

    public virtual void Stop()
    {
        _source.Stop();
    }
}
