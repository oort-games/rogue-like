using UnityEngine;

public class SoundBGM : SoundBase
{
    public override void Play(AudioClip clip)
    {
        base.Play(clip);
        _source.loop = true;
    }
}
