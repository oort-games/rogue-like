using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class UIScrollViewContent : Selectable
{
    [SerializeField] GameObject _highlight;
    [SerializeField] RectTransformOffset _margin;

    RectTransform _rectTransform;
    ScrollRect _scrollRect;
    UIPopup _parentPopup;

    protected override void Awake()
    {
        base.Awake();
        if (Application.isPlaying == false) return;
        _rectTransform = GetComponent<RectTransform>();
        _scrollRect = GetComponentInParent<ScrollRect>();
        _parentPopup = GetComponentInParent<UIPopup>();
        SetSelectedVisual(false);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        SetSelectedVisual(false);
    }

    #region Pointer / Selection
    public override void OnSelect(BaseEventData eventData)
    {
        SetSelectedVisual(true);
        if (_scrollRect == null) return;
        _rectTransform.EnsureVisibleInScrollView(_scrollRect, _margin);
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
            if (UIManager.Instance.IsLastPopup(_parentPopup.gameObject) == false) yield break;
            SetSelectedVisual(false);
        }
    }
    #endregion

    #region Navigation (공통: ↑/↓, 파생: ←/→)
    public override void OnMove(AxisEventData eventData)
    {
        Selectable selectable = null;
        switch (eventData.moveDir)
        {
            case MoveDirection.Right:
                selectable = FindSelectableOnRight();
                break;
            case MoveDirection.Up:
                selectable = FindSelectableOnUp();
                break;
            case MoveDirection.Left:
                selectable = FindSelectableOnLeft();
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

    protected void Navigate(AxisEventData eventData, Selectable sel)
    {
        if (sel != null && sel.IsActive())
            eventData.selectedObject = sel.gameObject;
    }
    #endregion

    #region Virtual Methods
    /// <summary>선택 시 비주얼 토글 (파생 클래스가 필요한 오브젝트 함께 제어)</summary>
    protected virtual void SetSelectedVisual(bool isSelected)
    {
        _highlight.SetActive(isSelected);
    }
    #endregion
}
