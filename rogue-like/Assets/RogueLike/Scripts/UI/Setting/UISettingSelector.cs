using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UISettingSelector : UISettingBase
{
    [Header("Selector")]
    [SerializeField] Button _prevButton;
    [SerializeField] Button _nextButton;
    [SerializeField] TextMeshProUGUI _valueText;
    [SerializeField] string[] _options;

    int _currentIndex;

    public string GetCurrentOption() => _options[_currentIndex];
    
    #region Overrides
    protected override void HandleHorizontal(MoveDirection dir)
    {
        if (dir == MoveDirection.Left) ChangeOption(-1);
        if (dir == MoveDirection.Right) ChangeOption(1);
    }

    protected override void RegisterButtons()
    {
        _prevButton.onClick.AddListener(() => { ChangeOption(-1); EventSystem.current.SetSelectedGameObject(gameObject); });
        _nextButton.onClick.AddListener(() => { ChangeOption(1); EventSystem.current.SetSelectedGameObject(gameObject); });
        UpdateUI();
    }

    protected override void SetSelectedVisual(bool isSelected)
    {
        _highlight.SetActive(isSelected);
        _prevButton.gameObject.SetActive(isSelected);
        _nextButton.gameObject.SetActive(isSelected);
    }
    #endregion

    #region Private
    void ChangeOption(int delta)
    {
        _currentIndex = (_currentIndex + delta + _options.Length) % _options.Length;
        UpdateUI();
    }

    void UpdateUI() => _valueText.text = _options[_currentIndex];
    #endregion
}
