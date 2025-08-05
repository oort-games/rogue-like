using System;
using System.Linq;
using UnityEngine;

[AddComponentMenu("Settings/UI/Display/UI Display ScreenMode Selector")]
[RequireComponent(typeof(UISettingSelector))]
public class UIDisplayScreenModeSelector : MonoBehaviour
{
    UISettingSelector _selector;
    DisplayScreenMode[] _screenModes;

    private void Awake()
    {
        _screenModes = Enum.GetValues(typeof(DisplayScreenMode)).Cast<DisplayScreenMode>().ToArray();

        _selector = GetComponent<UISettingSelector>();
        _selector.Initialize(_screenModes.Select(screenMode => screenMode.ToCustomString()).ToArray(),
            Array.IndexOf(_screenModes, DisplayManager.Instance.GetScreenMode()),
            OnValueChanged);
        _selector.SetResetAction(ResetAction);
    }

    void OnValueChanged(int value)
    {
        DisplayManager.Instance.SetScreenMode((DisplayScreenMode)value);
    }

    void ResetAction()
    {
        _selector.SetIndexWithoutNotify(Array.IndexOf(_screenModes, DisplayManager.Instance.GetScreenMode()));
    }
}
