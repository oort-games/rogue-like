using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

[RequireComponent(typeof(Slider))]
public class UISoundMasterVolumeSlider : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _valueText;
    [SerializeField] UISoundMasterMuteToggle _muteToggle;

    [Space(10)]
    [SerializeField] UnityEvent _onAction;
    [Space(10)]
    [SerializeField] UnityEvent _offAction;

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
        float volume = SoundManager.Instance.GetMasterVolume();
        _slider.SetValueWithoutNotify(volume);

        if (_valueText != null)
            _valueText.text = ((int)volume).ToString();

        Mute(SoundManager.Instance.GetMasterMute());
    }

    void OnValueChanged(float value)
    {
        SoundManager.Instance.SetMasterVolume(value);

        if (_valueText != null)
            _valueText.text = ((int)value).ToString();

        if (SoundManager.Instance.GetMasterMute())
        {
            SoundManager.Instance.SetMasterMute(false);
            Mute(false);

            if (_muteToggle != null)
                _muteToggle.WakeUp();
        }
    }

    public void Mute(bool mute)
    {
        (mute ? _offAction : _onAction).Invoke();
    }
}