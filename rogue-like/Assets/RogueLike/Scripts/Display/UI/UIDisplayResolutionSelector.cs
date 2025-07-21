using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(UISettingSelector))]
public class UIDisplayResolutionSelector : MonoBehaviour
{
    UISettingSelector _selector;
    DisplayResolution[] _resolutions;

    private void Awake()
    {
        _resolutions = Enum.GetValues(typeof(DisplayResolution)).Cast<DisplayResolution>().ToArray();

        _selector = GetComponent<UISettingSelector>();
        _selector.SetOption(_resolutions.Select(resolution => resolution.ToCustomString()).ToArray());
        _selector.SetIndex(Array.IndexOf(_resolutions, DisplayManager.Instance.GetResolution()));
        _selector.SetAction(OnValueChanged);
    }

    private void OnValueChanged(int value)
    {
        DisplayManager.Instance.SetResolution((DisplayResolution)value);
    }
}
