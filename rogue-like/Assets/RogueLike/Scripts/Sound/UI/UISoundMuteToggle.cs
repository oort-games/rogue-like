using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class UISoundMuteToggle : MonoBehaviour
{
    [SerializeField] SoundType _type;
    [SerializeField] UISoundVolumeSlider _volumeSlider;
    
    Toggle _toggle;

    private void Awake()
    {
        _toggle = GetComponent<Toggle>();
        _toggle.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnEnable()
    {
        _toggle.SetIsOnWithoutNotify(SoundManager.Instance.GetMute(_type));
    }

    void OnValueChanged(bool value)
    {
        SoundManager.Instance.SetMute(_type, value);

        if (_volumeSlider != null)
            _volumeSlider.Mute(value);
    }

    public void WakeUp()
    {
        _toggle.SetIsOnWithoutNotify(false);
    }
}
