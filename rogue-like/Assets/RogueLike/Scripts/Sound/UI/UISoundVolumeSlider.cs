using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

[RequireComponent(typeof(Slider))]
public class UISoundVolumeSlider : MonoBehaviour
{
    [SerializeField] SoundType _type;
    [SerializeField] TextMeshProUGUI _valueText;
    [SerializeField] UISoundMuteToggle _muteToggle;

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
        float volume = SoundManager.Instance.GetVolume(_type);
        _slider.SetValueWithoutNotify(volume);
 
        if (_valueText != null)
            _valueText.text = ((int)volume).ToString();

        _slider.interactable = _type == SoundType.Master || !SoundManager.Instance.GetMute(SoundType.Master);
        Mute(SoundManager.Instance.GetMute(SoundType.Master) || SoundManager.Instance.GetMute(_type));
    }

    void OnValueChanged(float value)
    {
        SoundManager.Instance.SetVolume(_type, value);

        if (_valueText != null)
            _valueText.text = ((int)value).ToString();

        if (SoundManager.Instance.GetMute(_type))
        {
            SoundManager.Instance.SetMute(_type, false);
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
