using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class UISoundMuteToggle : MonoBehaviour
{
    public SoundData.Type soundType;
    public UISoundVolumeSlider volumeSlider;
    
    Toggle _toggle;

    private void Awake()
    {
        _toggle = GetComponent<Toggle>();
        _toggle.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnEnable()
    {
        bool mute = false;
        switch (soundType)
        {
            case SoundData.Type.BGM:
                mute = SoundManager.Instance.MuteBGM;
                break;
            case SoundData.Type.SFX:
                mute = SoundManager.Instance.MuteSFX;
                break;
        }
        _toggle.SetIsOnWithoutNotify(mute);
    }

    void OnValueChanged(bool value)
    {
        switch (soundType)
        {
            case SoundData.Type.BGM:
                SoundManager.Instance.MuteBGM = value;
                break;
            case SoundData.Type.SFX:
                SoundManager.Instance.MuteSFX = value;
                break;
        }

        if (volumeSlider != null)
        {
            volumeSlider.Mute(value);
        }
    }

    public void WakeUp()
    {
        _toggle.SetIsOnWithoutNotify(false);
    }
}
