using UnityEngine;

[AddComponentMenu("Settings/UI/Sound/UI Sound Slider")]
[RequireComponent(typeof(UISettingSlider))]
public class UISoundSlider : MonoBehaviour
{
    [SerializeField] SoundType _type;
    UISettingSlider _slider;

    private void Awake()
    {
        _slider = GetComponent<UISettingSlider>();
        _slider.Initialize(SoundManager.Instance.GetVolume(_type), SoundManager.MAX_VALUE, true, OnValueChanged);
        _slider.SetResetAction(ResetAction);
    }

    void OnValueChanged(float value)
    {
        SoundManager.Instance.SetVolume(_type, value);
    }

    void ResetAction()
    {
        _slider.SetValueWithoutNotify(SoundManager.Instance.GetVolume(_type));
    }
}
