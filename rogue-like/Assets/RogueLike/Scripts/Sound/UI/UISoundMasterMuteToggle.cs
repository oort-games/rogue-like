using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISoundMasterMuteToggle : MonoBehaviour
{
    [SerializeField] UISoundMasterVolumeSlider _volumeSlider;

    [SerializeField] List<UISoundVolumeSlider> _channelVolumeSliders;
    [SerializeField] List<UISoundMuteToggle> _channelMuteToggles;

    Toggle _toggle;
    
    private void Awake()
    {
        _toggle = GetComponent<Toggle>();
        _toggle.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnEnable()
    {
        _toggle.SetIsOnWithoutNotify(!SoundManager.Instance.GetMasterMute());
    }

    void OnValueChanged(bool value)
    {
        SoundManager.Instance.SetMasterMute(!value);

        if (_volumeSlider != null)
            _volumeSlider.Mute(!value);

        SetInteractable(value);
    }

    public void WakeUp()
    {
        _toggle.SetIsOnWithoutNotify(true);
        SetInteractable(true);
    }

    void SetInteractable(bool value)
    {
        foreach(var slider in _channelVolumeSliders)
        {
            slider.SetInteractable(value);
        }

        foreach (var toggle in _channelMuteToggles)
        {
            toggle.SetInteractable(value);
        }
    }
}
