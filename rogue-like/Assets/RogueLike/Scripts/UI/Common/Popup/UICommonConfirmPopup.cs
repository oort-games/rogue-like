using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System;

public class UICommonConfirmPopup : UIPopup
{
    [Header("Texts")]
    [SerializeField] TextMeshProUGUI _titleText;
    [SerializeField] TextMeshProUGUI _infoText;
    [SerializeField] TextMeshProUGUI _confirmText;
    [SerializeField] TextMeshProUGUI _closeText;

    [Header("Buttons")]
    [SerializeField] Button _confirmButton;

    UnityAction _confirmAction;

    public void Initialize(UnityAction confirmAction, string titleStr, string infoStr, string confirmStr, string closeStr)
    {
        _confirmAction = confirmAction;

        _titleText.text = titleStr;
        _infoText.text = infoStr;
        _confirmText.text = confirmStr;
        _closeText.text = closeStr;

        _confirmButton.onClick.AddListener(confirmAction);
    }

    protected override void Start()
    {
        base.Start();
        UIManager.Instance.AddConfirmAction(ConfirmCallback);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (UIManager.Instance == null) return;
        UIManager.Instance.DeleteConfirmAction(ConfirmCallback);
    }

    void ConfirmCallback(InputAction.CallbackContext context)
    {
        _confirmAction?.Invoke();
    }
}
