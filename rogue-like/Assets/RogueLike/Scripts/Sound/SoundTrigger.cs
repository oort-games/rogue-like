using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    [SerializeField] SoundType _type;
    [SerializeField] AudioClip _clip;

    [SerializeField] bool _playOnStart;

    void Start()
    {
        if (_playOnStart)
            Play();
    }

    [ContextMenu("Play")]
    public void Play()
    {
        if (_clip != null)
            SoundManager.Instance.Play(_type, _clip);
    }
}