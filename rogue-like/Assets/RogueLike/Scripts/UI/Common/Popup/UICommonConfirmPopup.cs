using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class UICommonConfirmPopup : UIPopup
{
    [Header("Texts")]
    [SerializeField] TextMeshProUGUI _titleText;
    [SerializeField] TextMeshProUGUI _infoText;
    [SerializeField] TextMeshProUGUI _confirmText;
    [SerializeField] TextMeshProUGUI _closeText;

    [Header("Buttons")]
    [SerializeField] Button _confirmButton;

    [Header("Inputs")]
    [SerializeField] InputActionReference _confirmActionRef;

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

    private void OnEnable()
    {
        _confirmActionRef.action.performed += _ => _confirmAction?.Invoke();
        _confirmActionRef.action.Enable();
    }

    private void OnDisable()
    {
        _confirmActionRef.action.performed -= _ => _confirmAction?.Invoke();
        _confirmActionRef.action.Disable();
    }
}
