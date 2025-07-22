using UnityEngine;

[RequireComponent(typeof(UISettingSlider))]
public class UISoundSlider : MonoBehaviour
{
    [SerializeField] SoundType _type;
    UISettingSlider _slider;

    private void Awake()
    {
        _slider = GetComponent<UISettingSlider>();
        _slider.SetValue(SoundManager.Instance.GetVolume(_type));
        _slider.SetWholeNUmbers(true);
        _slider.SetMaxValue(SoundManager.MAX_VALUE);
        _slider.SetAction(OnValueChanged);
    }

    void OnValueChanged(float value)
    {
        SoundManager.Instance.SetVolume(_type, value);
    }
}
