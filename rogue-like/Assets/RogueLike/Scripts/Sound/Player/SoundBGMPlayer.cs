using UnityEngine;

public class SoundBGMPlayer : SoundPlayer
{
    public override void Play(AudioClip clip)
    {
        base.Play(clip);
        _source.loop = true;
    }
}