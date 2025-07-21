using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Settings/UI/Display/UI Display LimitFrameRate Toggle")]
[RequireComponent(typeof(Toggle))]
public class UIDisplayLimitFrameRateToggle : MonoBehaviour
{
    Toggle _toggle;

    private void Awake()
    {
        _toggle = GetComponent<Toggle>();
        _toggle.onValueChanged.AddListener(OnValueChanged);
    }

    private void Start()
    {
        _toggle.SetIsOnWithoutNotify(DisplayManager.Instance.GetLimitFrameRate());
    }

    void OnValueChanged(bool value)
    {
        DisplayManager.Instance.SetLimitFrameRate(value);
    }
}
