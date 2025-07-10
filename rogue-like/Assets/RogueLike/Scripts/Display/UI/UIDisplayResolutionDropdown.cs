using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[AddComponentMenu("Settings/UI/Display/UI Display Resolution Dropdown")]
[RequireComponent(typeof(TMP_Dropdown))]
public class UIDisplayResolutionDropdown : MonoBehaviour
{
    TMP_Dropdown _dropDown;
    List<DisplayResolution> _resolutions;

    private void Awake()
    {
        _dropDown = GetComponent<TMP_Dropdown>();

        _resolutions = Enum.GetValues(typeof(DisplayResolution)).Cast<DisplayResolution>().ToList();
        _dropDown.ClearOptions();
        _dropDown.AddOptions(_resolutions.Select(resolution => resolution.ToCustomString()).ToList());

        _dropDown.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnEnable()
    {
        _dropDown.SetValueWithoutNotify(_resolutions.IndexOf(DisplayManager.Instance.GetResolution()));
        _dropDown.RefreshShownValue();
    }

    void OnValueChanged(int value)
    {
        DisplayManager.Instance.SetResolution((DisplayResolution)value);
    }
}