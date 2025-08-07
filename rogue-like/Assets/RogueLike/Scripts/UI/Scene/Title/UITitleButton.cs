using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class UITitleButton : Selectable
{
    [SerializeField] Button _button;
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] GameObject _highlight;
    [SerializeField] Color _normalColor;
    [SerializeField] Color _selectedColor;

    UnityAction _onClickAction;

    UITitleScene _titleScene;

    protected override void Awake()
    {
        base.Awake();
        if (Application.isPlaying == false) return;
        _titleScene = GetComponentInParent<UITitleScene>();
        SetSelectedVisual(false);
    }

    protected override void Start()
    {
        base.Start();
        _button.onClick.AddListener(() =>
        {
            _onClickAction.Invoke();
        });
    }

    #region Pointer / Selection
    public override void OnSelect(BaseEventData eventData)
    {
        SetSelectedVisual(true);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        StartCoroutine(DelayedDeselectCheck());
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    IEnumerator DelayedDeselectCheck()
    {
        yield return null;
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
        else
        {
            SetSelectedVisual(false);
        }
    }

    void SetSelectedVisual(bool isSelected)
    {
        _highlight.SetActive(isSelected);
        _text.color = isSelected ? _selectedColor : _normalColor;
        if (isSelected)
        {
            _titleScene.SetAction(_onClickAction);
        }
    }
    #endregion

    #region Navigation
    public override void OnMove(AxisEventData eventData)
    {
        Selectable selectable = null;
        switch (eventData.moveDir)
        {
            case MoveDirection.Up:
                selectable = FindSelectableOnUp();
                break;
            case MoveDirection.Down:
                selectable = FindSelectableOnDown();
                break;
        }
        if (selectable != null)
        {
            if (selectable.transform.parent != transform.parent) return;
            SetSelectedVisual(false);
            Navigate(eventData, selectable);
        }
    }

    void Navigate(AxisEventData eventData, Selectable sel)
    {
        if (sel != null && sel.IsActive())
            eventData.selectedObject = sel.gameObject;
    }
    #endregion

    public void Initialize(UnityAction onClickAction)
    {
        _onClickAction = onClickAction;
    }
}
