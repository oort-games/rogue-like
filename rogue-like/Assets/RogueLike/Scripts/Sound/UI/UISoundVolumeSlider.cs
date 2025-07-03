using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class UISoundVolumeSlider : MonoBehaviour
{
    public SoundType type;
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
        float volume = SoundManager.Instance.GetVolume(type);
        _slider.SetValueWithoutNotify(volume);
 
        if (valueText != null)
            valueText.text = ((int)volume).ToString();

        Mute(SoundManager.Instance.GetMute(type));
    }

    void OnValueChanged(float value)
    {
        SoundManager.Instance.SetVolume(type, value);

        if (valueText != null)
            valueText.text = ((int)value).ToString();

        if (SoundManager.Instance.GetMute(type))
        {
            SoundManager.Instance.SetMute(type, false);
            Mute(false);

            if (muteToggle != null)
                muteToggle.WakeUp();
        }
    }

    public void Mute(bool mute)
    {
        _slider.handleRect.GetComponent<Image>().color = mute ? Color.red : Color.green;
    }
}
