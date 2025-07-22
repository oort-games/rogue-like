using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(UISettingSelector))]
public class UIDisplayLimtFrameRateSelector : MonoBehaviour
{
    [SerializeField] UISettingBase _targetFrameSelector;

    UISettingSelector _selector;
    UIToggleState[] _toggleStates;

    private void Awake()
    {
        _toggleStates = Enum.GetValues(typeof(UIToggleState)).Cast<UIToggleState>().ToArray();
        bool isLimit = DisplayManager.Instance.GetLimitFrameRate();

        _selector = GetComponent<UISettingSelector>();
        _selector.SetOption(_toggleStates.Select(toggleStates => toggleStates.ToCustomString()).ToArray());
        _selector.SetIndex(isLimit == true ? 1 : 0);
        _selector.SetAction(OnValueChanged);

        _targetFrameSelector.SetEnable(isLimit);
    }

    private void OnValueChanged(int value)
    {
        bool isLimit = value == 1;

        DisplayManager.Instance.SetLimitFrameRate(isLimit);
        _targetFrameSelector.SetEnable(isLimit);
    }
}
