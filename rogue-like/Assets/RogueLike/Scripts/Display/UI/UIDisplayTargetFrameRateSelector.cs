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
        _selector.Initialize(_targetFrameRates.Select(targetFrameRate => targetFrameRate.ToCustomString()).ToArray(),
            Array.IndexOf(_targetFrameRates, DisplayManager.Instance.GetTargetFrameRate()),
            OnValueChanged);
        _selector.SetResetAction(ResetAction);
    }

    void OnValueChanged(int value)
    {
        DisplayManager.Instance.SetTargetFrameRate((DisplayTargetFrameRate)value);
    }

    void ResetAction()
    {
        _selector.SetIndexWithoutNotify(Array.IndexOf(_targetFrameRates, DisplayManager.Instance.GetTargetFrameRate()));
    }
}
