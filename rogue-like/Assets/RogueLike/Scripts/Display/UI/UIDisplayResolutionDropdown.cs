using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
        _dropDown.AddOptions(_resolutions.Select(index =>
        {
            Resolution resolution = index.ToResolution();
            return $"{resolution.width} x {resolution.height}";
        }).ToList());

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
