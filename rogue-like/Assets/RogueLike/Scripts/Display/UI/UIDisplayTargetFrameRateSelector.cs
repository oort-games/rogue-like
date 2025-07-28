using System;
using System.Linq;
using UnityEngine;

[AddComponentMenu("Settings/UI/Display/UI Display TargetFrameRate Selector")]
[RequireComponent(typeof(UISettingSelector))]
public class UIDisplayTargetFrameRateSelector : MonoBehaviour
{
    UISettingSelector _selector;
    DisplayTargetFrameRate[] _targetFrameRates;

    private void Awake()
    {
        _targetFrameRates = Enum.GetValues(typeof(DisplayTargetFrameRate)).Cast<DisplayTargetFrameRate>().ToArray();

        _selector = GetComponent<UISettingSelector>();
        _selector.SetOption(_targetFrameRates.Select(targetFrameRate => targetFrameRate.ToCustomString()).ToArray());
        _selector.SetIndex(Array.IndexOf(_targetFrameRates, DisplayManager.Instance.GetTargetFrameRate()));
        _selector.SetAction(OnValueChanged);
    }

    void OnValueChanged(int value)
    {
        DisplayManager.Instance.SetTargetFrameRate((DisplayTargetFrameRate)value);
    }
}
