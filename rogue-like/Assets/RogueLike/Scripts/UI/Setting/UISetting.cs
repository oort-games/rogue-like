using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UISetting : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] Button applyButton;
    [SerializeField] Button resetButton;
    [SerializeField] Button closeButton;

    [Header("Inputs")]
    [SerializeField] InputActionReference applyAction;
    [SerializeField] InputActionReference resetAction;
    [SerializeField] InputActionReference closeAction;

    UISettingBase[] _settingList;

    private void Start()
    {
        applyButton.onClick.AddListener(OnClickApply);
        resetButton.onClick.AddListener(OnClickReset);
        closeButton.onClick.AddListener(OnClickClose);

        _settingList = GetComponentsInChildren<UISettingBase>(true);
    }

    private void OnEnable()
    {
        applyAction.action.performed += _ => OnClickApply();
        applyAction.action.Enable();

        resetAction.action.performed += _ => OnClickReset();
        resetAction.action.Enable();

        closeAction.action.performed += _ => OnClickClose();
        closeAction.action.Enable();
    }

    private void OnDisable()
    {
        applyAction.action.performed -= _ => OnClickApply();
        applyAction.action.Disable();

        resetAction.action.performed -= _ => OnClickReset();
        resetAction.action.Disable();

        closeAction.action.performed -= _ => OnClickClose();
        closeAction.action.Disable();
    }

    void OnClickApply()
    {
        Debug.Log("Apply");
    }

    void OnClickReset()
    {
        Debug.Log("Reset");
        
        SoundManager.Instance.ResetOption();
        DisplayManager.Instance.ResetOption();
        foreach (var setting in _settingList)
        {
            setting.ResetOption();
        }
    }

    void OnClickClose()
    {
        Debug.Log("Close");
    }
}
