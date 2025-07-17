using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[AddComponentMenu("Settings/UI/Sound/UI Sound Mute Toggle")]
[RequireComponent(typeof(Toggle))]
public class UISoundMuteToggle : MonoBehaviour
{
    [SerializeField] SoundType _type;
    [SerializeField] UISoundVolumeSlider _volumeSlider;

    [Header("Events")]
    [SerializeField] UnityEvent<bool> _onExtensionValueChanged;

    Toggle _toggle;

    private void Awake()
    {
        _toggle = GetComponent<Toggle>();
        _toggle.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnEnable()
    {
        _toggle.interactable = _type == SoundType.Master || !SoundManager.Instance.GetMute(SoundType.Master);
        _toggle.SetIsOnWithoutNotify(!SoundManager.Instance.GetMute(_type));
    }

    void OnValueChanged(bool value)
    {
        SoundManager.Instance.SetMute(_type, !value);

        if (_volumeSlider != null)
            _volumeSlider.Mute(!value);

        _onExtensionValueChanged?.Invoke(value);
    }

    public void WakeUp()
    {
        _toggle.SetIsOnWithoutNotify(true);
        _onExtensionValueChanged?.Invoke(true);
    }
}