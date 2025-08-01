using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class UISettingSelector : UISettingBase
{
    [Header("Selector")]
    [SerializeField] Button _prevButton;
    [SerializeField] Button _nextButton;
    [SerializeField] TextMeshProUGUI _valueText;
    [SerializeField] string[] _options;
    [SerializeField] bool _immediately = true;
    [SerializeField] GameObject _change;

    int _currentIndex;
    int _prevIndex;
    bool _isApplyEventBound;

    UnityAction<int> _onValueChanged;


    #region Unity Life-cycle
    protected override void Start()
    {
        base.Start();
        if (Application.isPlaying == false) return;
        UpdateUI();
    }
    #endregion

    #region Overrides
    protected override void HandleHorizontal(MoveDirection dir)
    {
        if (dir == MoveDirection.Left) ChangeOption(-1);
        if (dir == MoveDirection.Right) ChangeOption(1);
    }

    protected override void RegisterButtons()
    {
        _prevButton.onClick.AddListener(() => { ChangeOption(-1); });
        _nextButton.onClick.AddListener(() => { ChangeOption(1); });
    }

    protected override void SetSelectedVisual(bool isSelected)
    {
        base.SetSelectedVisual(isSelected);
        _prevButton.gameObject.SetActive(isSelected);
        _nextButton.gameObject.SetActive(isSelected);
    }
    #endregion

    #region Private
    void ChangeOption(int delta)
    {
        if (_enable == false) return;
        _currentIndex = (_currentIndex + delta + _options.Length) % _options.Length;
        
        if (_immediately == true)
        {
            Apply();
        }
        else
        {
            UpdateUI();

            bool changed = IsChanged();
            if (changed == _isApplyEventBound) return;

            _isApplyEventBound = changed;
            if (changed)
                _settingPopup.OnApply += Apply;
            else
                _settingPopup.OnApply -= Apply;
        }
    }

    void UpdateUI()
    {
        _valueText.text = _options[_currentIndex];
        if (_immediately == false)
            _change.SetActive(IsChanged());
        else
            _change.SetActive(false);
    }

    bool IsChanged()
    {
        return _currentIndex != _prevIndex;
    }

    void Apply()
    {
        _isApplyEventBound = false;
        _onValueChanged?.Invoke(_currentIndex);
        _prevIndex = _currentIndex;
        UpdateUI();
    }

    #endregion

    #region Public
    public void SetOption(string[] options)
    {
        _options = options;
    }

    public void SetIndex(int index)
    {
        _currentIndex = index;
        _prevIndex = index;
    }

    public void SetIndexWithoutNotify(int index)
    {
        SetIndex(index);
        UpdateUI();
        if (_immediately == false && _isApplyEventBound)
        {
            _isApplyEventBound = false;
            _settingPopup.OnApply -= Apply;
        }
    }

    public void SetAction(UnityAction<int> onValueChanged)
    {
        _onValueChanged = onValueChanged;
    }
    #endregion
}
