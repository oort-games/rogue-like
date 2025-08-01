using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class UICommonConfirmPopup : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] TextMeshProUGUI _titleText;
    [SerializeField] TextMeshProUGUI _infoText;
    [SerializeField] TextMeshProUGUI _confirmText;
    [SerializeField] TextMeshProUGUI _closeText;

    [Header("Buttons")]
    [SerializeField] Button _confirmButton;
    [SerializeField] Button _closeButton;

    [Header("Inputs")]
    [SerializeField] InputActionReference _confirmActionRef;
    [SerializeField] InputActionReference _closeActionRef;

    UIPopup _popup;
    UnityAction _confirmAction;

    public void Initialize(UIPopup popup, UnityAction confirmAction, string titleStr, string infoStr, string confirmStr, string closeStr)
    {
        _popup = popup;
        _confirmAction = confirmAction;

        _titleText.text = titleStr;
        _infoText.text = infoStr;
        _confirmText.text = confirmStr;
        _closeText.text = closeStr;

        _confirmButton.onClick.AddListener(confirmAction);
        _closeButton.onClick.AddListener(popup.Close);
    }

    private void OnEnable()
    {
        _confirmActionRef.action.performed += _ => _confirmAction?.Invoke();
        _confirmActionRef.action.Enable();

        _closeActionRef.action.performed += _ => _popup.Close();
        _closeActionRef.action.Enable();
    }

    private void OnDisable()
    {
        _confirmActionRef.action.performed -= _ => _confirmAction?.Invoke();
        _confirmActionRef.action.Disable();

        _closeActionRef.action.performed -= _ => _popup.Close();
        _closeActionRef.action.Disable();
    }
}
