using UnityEngine;
using TMPro;

public class UISettingTabButton : UITabButton
{
    [Header("Extensions")]
    public TextMeshProUGUI _text;
    [SerializeField] Color _defaultColor;
    [SerializeField] Color _highlightColor;

    public override void SetSelected(bool isOn)
    {
        base.SetSelected(isOn);
        _text.color = isOn ? _highlightColor : _defaultColor;
    }
}
