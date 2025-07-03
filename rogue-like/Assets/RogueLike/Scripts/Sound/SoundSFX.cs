using UnityEngine;

public class SoundSFX : SoundBase
{
    public override void Play(AudioClip clip)
    {
        _source.PlayOneShot(clip);
    }
}
