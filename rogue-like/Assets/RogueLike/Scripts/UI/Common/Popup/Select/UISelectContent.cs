using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UISelectContent : UIScrollViewContent
{
    [SerializeField] Button _button;
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] Color _defaultColor;
    [SerializeField] Color _highlightColor;

    int _index;
    UnityAction<int> _onClickAction;
    UnityAction<int> _onSelectAction;

    protected override void Start()
    {
        base.Start();
        if (Application.isPlaying == false) return;
        _button.onClick.AddListener(() =>
        {
            _onClickAction.Invoke(_index);
            _parentPopup.Close();
            Debug.Log($"Click {_text.text}"); 
        });
    }

    protected override void SetSelectedVisual(bool isSelected)
    {
        base.SetSelectedVisual(isSelected);
        _text.color = isSelected ? _highlightColor : _defaultColor;
        if (isSelected)
            _onSelectAction.Invoke(_index);
    }

    public void Initialize(int index, string str, UnityAction<int> onClickAction, UnityAction<int> onSelectAction)
    {
        _index = index;
        _text.text = str;
        _onClickAction = onClickAction;
        _onSelectAction = onSelectAction;
    }
}