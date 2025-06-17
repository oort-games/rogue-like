using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class UISoundVolumeSlider : MonoBehaviour
{
    public SoundData.Type soundType;
    public TextMeshProUGUI valueText;

    Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _slider.wholeNumbers = true;
        _slider.maxValue = SoundManager.MAX_VALUE;
        _slider.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnEnable()
    {
        float volume = 100f;
        bool mute = false;
        switch (soundType)
        {
            case SoundData.Type.BGM:
                volume = SoundManager.Instance.VolumeBGM;
                mute = SoundManager.Instance.MuteBGM;
                break;
            case SoundData.Type.SFX:
                volume = SoundManager.Instance.VolumeSFX;
                mute = SoundManager.Instance.MuteSFX;
                break;
        }
        _slider.SetValueWithoutNotify(volume);
        _slider.interactable = !mute;

        if (valueText != null)
        {
            valueText.text = ((int)volume).ToString();
        }
    }

    void OnValueChanged(float value)
    {
        switch (soundType)
        {
            case SoundData.Type.BGM:
                SoundManager.Instance.VolumeBGM = value;
                break;
            case SoundData.Type.SFX:
                SoundManager.Instance.VolumeSFX = value;
                break;
        }

        if (valueText != null)
        {
            valueText.text = ((int)value).ToString();
        }
    }
}
