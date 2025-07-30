using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class UISettingSlider : UISettingBase
{
    [Header("Slider")]
    [SerializeField] UILongPressButton _prevButton;
    [SerializeField] UILongPressButton _nextButton;
    [SerializeField] Slider _slider;
    [SerializeField] TextMeshProUGUI _valueText;
    [SerializeField] float _step = 1f;
    [SerializeField] float _repeatRate = 0.025f;

    [Header("Deco")]
    [SerializeField] Image _sliderFillImage;
    [SerializeField] Color _defaultColor;
    [SerializeField] Color _highlightColor;

    UnityAction<float> _onValueChanged;

    #region Unity Life-cycle
    protected override void Start()
    {
        base.Start();
        if (Application.isPlaying == false) return;
        _prevButton.SetSetRepeatRate(_repeatRate);
        _nextButton.SetSetRepeatRate(_repeatRate);

        _slider.onValueChanged.AddListener(OnValueChanged);
        UpdateUI();
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
        base.SetSelectedVisual(isSelected);
        _prevButton.gameObject.SetActive(isSelected);
        _nextButton.gameObject.SetActive(isSelected);
        _sliderFillImage.color = isSelected ? _highlightColor : _defaultColor;
    }
    #endregion

    #region Private
    void ClickDelta(float delta)
    {
        if (_enable == false) return;
        _slider.value += delta;
    }

    void UpdateUI() => _valueText.text = $"{_slider.value}";

    void OnValueChanged(float value)
    {
        _onValueChanged?.Invoke(value);
        UpdateUI();
    }
    #endregion

    #region Public
    public void SetValue(float value)
    {
        _slider.value = value;
    }

    public void SetValueWithoutNotify(float value)
    {
        _slider.SetValueWithoutNotify(value);
        UpdateUI();
    }

    public void SetWholeNUmbers(bool value)
    {
        _slider.wholeNumbers = value;
    }

    public void SetMaxValue(float value)
    {
        _slider.maxValue = value;
    }

    public void SetAction(UnityAction<float> onValueChanged)
    {
        _onValueChanged = onValueChanged;
    }
    #endregion
}
