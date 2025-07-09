using UnityEngine;

public class SoundSFXPlayer : SoundPlayer
{
    public override void Play(AudioClip clip)
    {
        _source.PlayOneShot(clip);
    }
}
