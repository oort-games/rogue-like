using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Settings/UI/Display/UI Display VSync Toggle")]
[RequireComponent(typeof(Toggle))]
public class UIDisplayVSyncToggle : MonoBehaviour
{
    Toggle _toggle;

    void Awake()
    {
        _toggle = GetComponent<Toggle>();
        _toggle.onValueChanged.AddListener(OnValueChanged);
    }

    void OnEnable()
    {
        _toggle.SetIsOnWithoutNotify(DisplayManager.Instance.GetVSync());
    }

    void OnValueChanged(bool value)
    {
        DisplayManager.Instance.SetVSync(value);
    }
}
