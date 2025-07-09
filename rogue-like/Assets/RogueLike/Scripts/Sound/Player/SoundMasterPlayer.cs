using UnityEngine;

public class SoundMasterPlayer : SoundPlayer
{
    public override void Play(AudioClip clip)
    {
        _source.PlayOneShot(clip);
    }
}
