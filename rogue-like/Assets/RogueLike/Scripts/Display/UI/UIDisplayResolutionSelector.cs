using System;
using System.Linq;
using UnityEngine;

[AddComponentMenu("Settings/UI/Display/UI Display Resolution Selector")]
[RequireComponent(typeof(UISettingSelector))]
public class UIDisplayResolutionSelector : MonoBehaviour
{
    UISettingSelector _selector;
    DisplayResolution[] _resolutions;

    private void Awake()
    {
        _resolutions = Enum.GetValues(typeof(DisplayResolution)).Cast<DisplayResolution>().ToArray();

        _selector = GetComponent<UISettingSelector>();
        _selector.Initialize(_resolutions.Select(resolution => resolution.ToCustomString()).ToArray(),
            Array.IndexOf(_resolutions, DisplayManager.Instance.GetResolution()),
            OnValueChanged);
        _selector.SetResetAction(ResetAction);
    }

    void OnValueChanged(int value)
    {
        DisplayManager.Instance.SetResolution((DisplayResolution)value);
    }

    void ResetAction()
    {
        _selector.SetIndexWithoutNotify(Array.IndexOf(_resolutions, DisplayManager.Instance.GetResolution()));
    }
}
