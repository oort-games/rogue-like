using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public abstract class UISettingBase : Selectable
{
    [Header("Common")]
    [SerializeField] protected TextMeshProUGUI _headerText;
    [SerializeField] protected GameObject _highlight;
    [SerializeField] protected GameObject _dim;
    [SerializeField] protected GameObject _block;
    [SerializeField] RectTransformOffset _margin;

    protected bool _enable = true;

    RectTransform _rectTransform;
    ScrollRect _scrollRect;
    UnityAction _resetAction;

    #region Unity Life-cycle
    protected override void Awake()
    {
        base.Awake();
        _rectTransform = GetComponent<RectTransform>();
        if (Application.isPlaying == false) return;
        SetSelectedVisual(false);
    }

    protected override void Start()
    {
        base.Start();
        if (Application.isPlaying == false) return;
        _scrollRect = GetComponentInParent<ScrollRect>();
        RegisterButtons();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        SetSelectedVisual(false);
    }
    #endregion

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
            case MoveDirection.Up:
                selectable = FindSelectableOnUp();
                Navigate(eventData, selectable);
                if (selectable != null)
                    SetSelectedVisual(false);
                break;
            case MoveDirection.Down:
                selectable = FindSelectableOnDown();
                Navigate(eventData, FindSelectableOnDown());
                if (selectable != null)
                    SetSelectedVisual(false);
                break;
            case MoveDirection.Left:
            case MoveDirection.Right:
                if (EventSystem.current.currentSelectedGameObject == gameObject) HandleHorizontal(eventData.moveDir);
                break;
        }
    }

    void Navigate(AxisEventData eventData, Selectable sel)
    {
        if (sel != null && sel.IsActive())
            eventData.selectedObject = sel.gameObject;
    }
    #endregion

    #region Abstract Methods
    /// <summary>�� / �� �Է� ó�� (�Ļ� Ŭ�������� �ٸ�)</summary>
    protected abstract void HandleHorizontal(MoveDirection dir);

    /// <summary>��ư onClick �� �Ļ� Ŭ�������� ����</summary>
    protected abstract void RegisterButtons();
    #endregion

    #region Virtual Methods
    /// <summary>���� �� ���־� ��� (�Ļ� Ŭ������ �ʿ��� ������Ʈ �Բ� ����)</summary>
    protected virtual void SetSelectedVisual(bool isSelected)
    {
        _highlight.SetActive(isSelected);
        _dim.SetActive(!isSelected);
    }
    #endregion

    #region Public
    public void SetEnable(bool enable)
    {
        _enable = enable;
        _block.SetActive(!enable);
    }

    public void SetResetAction(UnityAction resetAction)
    {
        _resetAction = resetAction;
    }

    public void ResetOption()
    {
        _resetAction?.Invoke();
    }
    #endregion
}
