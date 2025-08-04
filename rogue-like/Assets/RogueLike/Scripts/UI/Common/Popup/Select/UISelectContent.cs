using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UISelectContent : UIScrollViewContent
{
    [SerializeField] Button _button;
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] Color _defaultColor;
    [SerializeField] Color _highlightColor;

    protected override void Start()
    {
        base.Start();
        if (Application.isPlaying == false) return;
        _button.onClick.AddListener(() => { Debug.Log($"Click {_text.text}"); });
    }

    protected override void SetSelectedVisual(bool isSelected)
    {
        base.SetSelectedVisual(isSelected);
        _text.color = isSelected ? _highlightColor : _defaultColor;
    }

    public void Initialize(string str)
    {
        _text.text = str;
    }
}