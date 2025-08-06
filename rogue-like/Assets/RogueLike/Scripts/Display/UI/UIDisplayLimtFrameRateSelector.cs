using System;
using System.Linq;
using UnityEngine;

[AddComponentMenu("Settings/UI/Display/UI Display LimitFrameRate Selector")]
[RequireComponent(typeof(UISettingSelector))]
public class UIDisplayLimtFrameRateSelector : MonoBehaviour
{
    [SerializeField] UISettingContent _targetFrameSelector;

    UISettingSelector _selector;
    UIToggleState[] _toggleStates;

    private void Awake()
    {
        _toggleStates = Enum.GetValues(typeof(UIToggleState)).Cast<UIToggleState>().ToArray();
        bool isLimit = DisplayManager.Instance.GetLimitFrameRate();

        _selector = GetComponent<UISettingSelector>();
        _selector.Initialize(_toggleStates.Select(toggleStates => toggleStates.ToCustomString()).ToArray(),
            isLimit == true ? 1 : 0,
            OnValueChanged);
        _selector.SetResetAction(ResetAction);

        _targetFrameSelector.SetEnable(isLimit);
    }

    void OnValueChanged(int value)
    {
        bool isLimit = value == 1;

        DisplayManager.Instance.SetLimitFrameRate(isLimit);
        _targetFrameSelector.SetEnable(isLimit);
    }

    void ResetAction()
    {
        bool isLimit = DisplayManager.Instance.GetLimitFrameRate();
        _selector.SetIndexWithoutNotify(isLimit == true ? 1 : 0);
        _targetFrameSelector.SetEnable(isLimit);
    }
}
