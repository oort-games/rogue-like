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

    }
    #endregion

    void Show()
    {
        Debug.Log("Show");
    }

    void Show(InputAction.CallbackContext context)
    {
        Show();
    }
}
