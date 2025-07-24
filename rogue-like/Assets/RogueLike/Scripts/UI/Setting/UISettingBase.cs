using System.Collections;
using UnityEngine;
using UnityEngine.UI;
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
            Debug.Log("???");
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
        else
            SetSelectedVisual(false);
    }
    #endregion

    #region Navigation (공통: ↑/↓, 파생: ←/→)
    public override void OnMove(AxisEventData eventData)
    {
        switch (eventData.moveDir)
        {
            case MoveDirection.Up:
                Navigate(eventData, FindSelectableOnUp());
                SetSelectedVisual(false);
                break;
            case MoveDirection.Down:
                Navigate(eventData, FindSelectableOnDown()); 
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
    /// <summary>← / → 입력 처리 (파생 클래스마다 다름)</summary>
    protected abstract void HandleHorizontal(MoveDirection dir);

    /// <summary>버튼 onClick 등 파생 클래스에서 연결</summary>
    protected abstract void RegisterButtons();
    #endregion

    #region Virtual Methods
    /// <summary>선택 시 비주얼 토글 (파생 클래스가 필요한 오브젝트 함께 제어)</summary>
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
    #endregion
}
