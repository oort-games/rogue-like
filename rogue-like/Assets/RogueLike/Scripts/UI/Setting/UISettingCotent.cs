using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public abstract class UISettingCotent : UIScrollViewContent
{
    [Header("Common")]
    [SerializeField] GameObject _dim;
    [SerializeField] GameObject _block;

    protected bool _enable = true;
    protected UISettingPopup _settingPopup;

    UnityAction _resetAction;

    #region Unity Life-cycle
    protected override void Start()
    {
        base.Start();
        if (Application.isPlaying == false) return;
        UpdateUI();
        RegisterButtons();
        _settingPopup = GetComponentInParent<UISettingPopup>();
    }
    #endregion

    #region Navigation (공통: ↑/↓, 파생: ←/→)
    public override void OnMove(AxisEventData eventData)
    {
        if (UIManager.Instance.IsLastPopup(_settingPopup.gameObject) == false) return;
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
    #endregion

    #region Abstract Methods
    /// <summary>← / → 입력 처리 (파생 클래스마다 다름)</summary>
    protected abstract void HandleHorizontal(MoveDirection dir);

    /// <summary>버튼 onClick 등 파생 클래스에서 연결</summary>
    protected abstract void RegisterButtons();

    protected abstract void UpdateUI();
    #endregion

    #region Virtual Methods
    /// <summary>선택 시 비주얼 토글 (파생 클래스가 필요한 오브젝트 함께 제어)</summary>
    protected override void SetSelectedVisual(bool isSelected)
    {
        base.SetSelectedVisual(isSelected);
        _dim.SetActive(!isSelected);
        if (isSelected) InputManager.Instance.SetMoveRepeatRate(0.1f);
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
