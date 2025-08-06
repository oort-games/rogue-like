using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;

public class UISettingButton : UISettingContent
{
    [Header("External Selector")]
    [SerializeField] TextMeshProUGUI _valueText;
    [SerializeField] Button _button;
    
    string[] _options;
    int _currentIndex;
    string _titleLocalizationKey;

    UnityAction<int> _onValueChanged;

    #region Unity Life-cycle
    protected override void OnEnable()
    {
        base.OnEnable();
        if (Application.isPlaying == false) return;
        UIManager.Instance.AddConfirmAction(Show);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (Application.isPlaying == false || UIManager.Instance == null) return;
        UIManager.Instance.DeleteConfirmAction(Show);
    }
    #endregion
    #region Overrides
    protected override void HandleHorizontal(MoveDirection dir)
    {

    }

    protected override void RegisterButtons()
    {
        _button.onClick.AddListener(Show);
    }

    protected override void UpdateUI()
    {
        _valueText.text = _options[_currentIndex];
    }
    #endregion

    #region Public
    public void Initialize(string[] options, int index, UnityAction<int> onValueChanged)
    {
        _options = options;
        _currentIndex = index;
        _onValueChanged = onValueChanged;
    }

    public void SetIndexWithoutNotify(int index)
    {
        _currentIndex = index;
        UpdateUI();
    }

    public void SetTitleLocalizationKey(string key)
    {
        _titleLocalizationKey = key;
    }
    #endregion

    #region Private
    void Show()
    {
        Debug.Log("Show");
        UISelectPopup selectPopup = UIManager.Instance.OpenPopupUI<UISelectPopup>();
        selectPopup.Initialize(_titleLocalizationKey, _options, _currentIndex, OnValueChanged);
    }

    void Show(InputAction.CallbackContext context)
    {
        if (UIManager.Instance.IsLastPopup(_settingPopup.gameObject) == false) return;
        Show();
    }

    void OnValueChanged(int index)
    {
        _currentIndex = index;
        UpdateUI();
        _onValueChanged.Invoke(index);
    }
    #endregion
}
