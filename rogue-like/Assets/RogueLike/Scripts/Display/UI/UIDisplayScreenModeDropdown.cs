using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Dropdown))]
public class UIDisplayScreenModeDropdown : MonoBehaviour
{
    TMP_Dropdown _dropDown;
    List<DisplayScreenMode> _modes;

    private void Awake()
    {
        _dropDown = GetComponent<TMP_Dropdown>();

        _modes = Enum.GetValues(typeof(DisplayScreenMode)).Cast<DisplayScreenMode>().ToList();
        _dropDown.ClearOptions();
        _dropDown.AddOptions(_modes.Select(m => m.ToString()).ToList());

        _dropDown.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnEnable()
    {
        _dropDown.value = _modes.IndexOf(DisplayManager.Instance.GetMode());
        _dropDown.RefreshShownValue();
    }

    void OnValueChanged(int value)
    {
        DisplayManager.Instance.SetMode((DisplayScreenMode)value);
    }
}
