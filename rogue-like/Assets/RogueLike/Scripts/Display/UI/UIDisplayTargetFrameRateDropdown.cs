using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[AddComponentMenu("Settings/UI/Display/UI Display TargetFrameRate Dropdown")]
[RequireComponent(typeof(TMP_Dropdown))]
public class UIDisplayTargetFrameRateDropdown : MonoBehaviour
{
    TMP_Dropdown _dropDown;
    List<DisplayTargetFrameRate> _targetFrameRates;

    private void Awake()
    {
        _targetFrameRates = Enum.GetValues(typeof(DisplayTargetFrameRate)).Cast<DisplayTargetFrameRate>().ToList();
        
        _dropDown = GetComponent<TMP_Dropdown>();
        _dropDown.ClearOptions();
        _dropDown.AddOptions(_targetFrameRates.Select(targetFrameRate => targetFrameRate.ToCustomString()).ToList());
        _dropDown.onValueChanged.AddListener(OnValueChanged);
    }

    private void Start()
    {
        _dropDown.SetValueWithoutNotify(_targetFrameRates.IndexOf(DisplayManager.Instance.GetTargetFrameRate()));
        _dropDown.RefreshShownValue();
    }

    void OnValueChanged(int value)
    {
        DisplayManager.Instance.SetTargetFrameRate((DisplayTargetFrameRate)value);
    }
}