using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System;

public class UIConfirmPopup : UIPopup
{
    [Header("Texts")]
    [SerializeField] TextMeshProUGUI _titleText;
    [SerializeField] TextMeshProUGUI _infoText;
    [SerializeField] TextMeshProUGUI _confirmText;
    [SerializeField] TextMeshProUGUI _closeText;

    [Header("Buttons")]
    [SerializeField] Button _confirmButton;

    UnityAction _confirmAction;

    public void Initialize(UnityAction confirmAction, string titleKey, string infoKey, string confirmKey, string closeKey)
    {
        _confirmAction = confirmAction;

        _titleText.text = LocalizationManager.Instance.GetString(titleKey);
        _infoText.text = LocalizationManager.Instance.GetString(infoKey);
        _confirmText.text = LocalizationManager.Instance.GetString(confirmKey);
        _closeText.text = LocalizationManager.Instance.GetString(closeKey);

        _confirmButton.onClick.AddListener(confirmAction);
        _confirmButton.onClick.AddListener(Close);
    }

    protected override void Start()
    {
        base.Start();
        UIManager.Instance.AddConfirmAction(Confirm);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (UIManager.Instance == null) return;
        UIManager.Instance.DeleteConfirmAction(Confirm);
    }

    void Confirm(InputAction.CallbackContext context)
    {
        if (UIManager.Instance.IsLastPopup(gameObject) == false) return; 
        _confirmAction?.Invoke();
        Close();
    }
}
