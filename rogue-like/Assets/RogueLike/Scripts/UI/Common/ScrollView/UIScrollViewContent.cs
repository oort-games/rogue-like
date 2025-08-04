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

    protected override void Awake()
    {
        base.Awake();
        _rectTransform = GetComponent<RectTransform>();
        if (Application.isPlaying == false) return;
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
            SetSelectedVisual(false);
        }
    }
    #endregion

    #region Navigation (����: ��/��, �Ļ�: ��/��)
    public override void OnMove(AxisEventData eventData)
    {
        Selectable selectable;
        switch (eventData.moveDir)
        {
            default:
                selectable = FindSelectableOnUp();
                Navigate(eventData, selectable);
                if (selectable != null)
                    SetSelectedVisual(false);
                break;
        }
    }

    protected void Navigate(AxisEventData eventData, Selectable sel)
    {
        if (sel != null && sel.IsActive())
            eventData.selectedObject = sel.gameObject;
    }
    #endregion

    #region Virtual Methods
    /// <summary>���� �� ���־� ��� (�Ļ� Ŭ������ �ʿ��� ������Ʈ �Բ� ����)</summary>
    protected virtual void SetSelectedVisual(bool isSelected)
    {
        _highlight.SetActive(isSelected);
    }
    #endregion
}
