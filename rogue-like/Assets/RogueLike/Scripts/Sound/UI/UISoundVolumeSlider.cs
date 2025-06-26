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
    public UISoundMuteToggle muteToggle;    

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
        Mute(mute);

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
                if (muteToggle != null && SoundManager.Instance.MuteBGM)
                {
                    SoundManager.Instance.MuteBGM = false;
                    Mute(false);
                    muteToggle.WakeUp();
                }
                break;
            case SoundData.Type.SFX:
                SoundManager.Instance.VolumeSFX = value;
                if (muteToggle != null && SoundManager.Instance.MuteSFX)
                {
                    SoundManager.Instance.MuteSFX = false;
                    Mute(false);
                    muteToggle.WakeUp();
                }
                break;
        }

        if (valueText != null)
        {
            valueText.text = ((int)value).ToString();
        }
    }

    public void Mute(bool mute)
    {
        _slider.handleRect.GetComponent<Image>().color = mute ? Color.red : Color.green;
    }
}
