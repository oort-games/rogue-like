using System;
using System.Linq;
using UnityEngine;

[AddComponentMenu("Settings/UI/Display/UI Display VSync Selector")]
[RequireComponent(typeof(UISettingSelector))]
public class UIDisplayVSyncSelector : MonoBehaviour
{
    UISettingSelector _selector;
    UIToggleState[] _toggleStates;

    private void Awake()
    {
        _toggleStates = Enum.GetValues(typeof(UIToggleState)).Cast<UIToggleState>().ToArray();

        _selector = GetComponent<UISettingSelector>();
        _selector.SetOption(_toggleStates.Select(toggleStates => toggleStates.ToCustomString()).ToArray());
        _selector.SetIndex(DisplayManager.Instance.GetVSync() == true ? 1 : 0);
        _selector.SetAction(OnValueChanged);
    }

    private void OnValueChanged(int value)
    {
        DisplayManager.Instance.SetVSync(value == 1);
    }
}
