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
    [SerializeField] bool _immediately;

    int _currentIndex;
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
        _prevButton.onClick.AddListener(() => 
        { 
            ChangeOption(-1); 
            EventSystem.current.SetSelectedGameObject(gameObject); 
        });
        _nextButton.onClick.AddListener(() => 
        { 
            ChangeOption(1); 
            EventSystem.current.SetSelectedGameObject(gameObject); 
        });
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
        _onValueChanged?.Invoke(_currentIndex);
        UpdateUI();
    }

    void UpdateUI() => _valueText.text = _options[_currentIndex];
    #endregion

    #region Public
    public void SetOption(string[] options)
    {
        _options = options;
    }

    public void SetIndex(int index)
    {
        _currentIndex = index;
    }

    public void SetAction(UnityAction<int> onValueChanged)
    {
        _onValueChanged = onValueChanged;
    }
    #endregion
}
