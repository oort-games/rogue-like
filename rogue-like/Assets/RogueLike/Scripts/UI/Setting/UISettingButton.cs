using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;

public class UISettingButton : UISettingCotent
{
    [Header("External Selector")]
    [SerializeField] TextMeshProUGUI _valueText;
    [SerializeField] Button _button;
    
    string[] _options;
    int _currentIndex;

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
        //_valueText.text = _options[_currentIndex];
    }
    #endregion

    #region Private
    void Show()
    {
        Debug.Log("Show");
        UISelectPopup selectPopup = UIManager.Instance.OpenPopupUI<UISelectPopup>();
        selectPopup.Initialize(null, new string[] { "ÇÑ±¹¾î", "English" }, 0);
    }

    void Show(InputAction.CallbackContext context)
    {
        if (UIManager.Instance.IsLastPopup(_settingPopup.gameObject) == false) return;
        Show();
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
    }
    #endregion
}
