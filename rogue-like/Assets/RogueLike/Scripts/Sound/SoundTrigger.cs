using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    [SerializeField] SoundType _type;
    [SerializeField] AudioClip _clip;

    [SerializeField] bool _playOnStart;

    private void Start()
    {
        if (_playOnStart)
            Play();
    }

    public void Play()
    {
        if (_clip != null)
            SoundManager.Instance.Play(_type, _clip);
    }
}