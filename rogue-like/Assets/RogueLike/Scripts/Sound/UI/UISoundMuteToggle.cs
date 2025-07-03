using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class UISoundMuteToggle : MonoBehaviour
{
    public SoundType type;
    public UISoundVolumeSlider volumeSlider;
    
    Toggle _toggle;

    private void Awake()
    {
        _toggle = GetComponent<Toggle>();
        _toggle.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnEnable()
    {
        _toggle.SetIsOnWithoutNotify(SoundManager.Instance.GetMute(type));
    }

    void OnValueChanged(bool value)
    {
        SoundManager.Instance.SetMute(type, value);

        if (volumeSlider != null)
            volumeSlider.Mute(value);
    }

    public void WakeUp()
    {
        _toggle.SetIsOnWithoutNotify(false);
    }
}
