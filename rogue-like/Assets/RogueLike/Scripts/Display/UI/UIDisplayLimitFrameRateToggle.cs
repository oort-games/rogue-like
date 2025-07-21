using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Settings/UI/Display/UI Display LimitFrameRate Toggle")]
[RequireComponent(typeof(Toggle))]
public class UIDisplayLimitFrameRateToggle : MonoBehaviour
{
    Toggle _toggle;

    void Awake()
    {
        _toggle = GetComponent<Toggle>();
        _toggle.onValueChanged.AddListener(OnValueChanged);
    }

    void OnEnable()
    {
        _toggle.SetIsOnWithoutNotify(DisplayManager.Instance.GetLimitFrameRate());
    }

    void OnValueChanged(bool value)
    {
        DisplayManager.Instance.SetLimitFrameRate(value);
    }
}
