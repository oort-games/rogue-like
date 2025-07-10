using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[AddComponentMenu("Settings/UI/Display/UI Display ScreenMode Dropdown")]
[RequireComponent(typeof(TMP_Dropdown))]
public class UIDisplayScreenModeDropdown : MonoBehaviour
{
    TMP_Dropdown _dropDown;
    List<DisplayScreenMode> _screenModes;

    private void Awake()
    {
        _dropDown = GetComponent<TMP_Dropdown>();

        _screenModes = Enum.GetValues(typeof(DisplayScreenMode)).Cast<DisplayScreenMode>().ToList();
        _dropDown.ClearOptions();
        _dropDown.AddOptions(_screenModes.Select(screenMode => screenMode.ToCustomString()).ToList());

        _dropDown.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnEnable()
    {
        _dropDown.SetValueWithoutNotify(_screenModes.IndexOf(DisplayManager.Instance.GetScreenMode()));
        _dropDown.RefreshShownValue();
    }

    void OnValueChanged(int value)
    {
        DisplayManager.Instance.SetScreenMode((DisplayScreenMode)value);
    }
}