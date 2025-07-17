using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class UISettingSlider : UISettingBase
{
    [Header("Slider")]
    [SerializeField] UILongPressButton _prevButton;
    [SerializeField] UILongPressButton _nextButton;
    [SerializeField] Slider _slider;
    [SerializeField] float _step = 1f;
    [SerializeField] float _repeatRate = 0.025f;

    #region Unity Life-cycle
    protected override void Start()
    {
        base.Start();
        if (Application.isPlaying == false) return;
        _prevButton.SetSetRepeatRate(_repeatRate);
        _nextButton.SetSetRepeatRate(_repeatRate);
    }
    #endregion

    #region Overrides
    protected override void HandleHorizontal(MoveDirection dir)
    {
        if (dir == MoveDirection.Left) ClickDelta(-_step);
        if (dir == MoveDirection.Right) ClickDelta(_step);
        InputManager.Instance.SetMoveRepeatRate(_repeatRate);
    }

    protected override void RegisterButtons()
    {
        _prevButton.onClick.AddListener(() => ClickDelta(-_step));
        _prevButton.onLongPress.AddListener(() => ClickDelta(-_step));
        _prevButton.onLongPressRepeat.AddListener(() => ClickDelta(-_step));

        _nextButton.onClick.AddListener(() => ClickDelta(_step));
        _nextButton.onLongPress.AddListener(() => ClickDelta(_step));
        _nextButton.onLongPressRepeat.AddListener(() => ClickDelta(_step));
    }

    protected override void SetSelectedVisual(bool isSelected)
    {
        _highlight.SetActive(isSelected);
        _prevButton.gameObject.SetActive(isSelected);
        _nextButton.gameObject.SetActive(isSelected);

        if (!isSelected)
            InputManager.Instance.SetMoveRepeatRate(0.1f);
    }
    #endregion

    #region Private
    void ClickDelta(float delta) => _slider.value += delta;
    #endregion
}
