using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UISetting : UIPopup
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

    int _applyActionCount = 0;
    event Action _onApply;
    public event Action OnApply
    {
        add
        {
            _onApply += value;
            _applyActionCount++;
            applyButton.gameObject.SetActive(_applyActionCount > 0);
        }
        remove
        {
            _onApply -= value;
            _applyActionCount--;
            applyButton.gameObject.SetActive(_applyActionCount > 0);
        }
    }

    private void Start()
    {
        applyButton.onClick.AddListener(OnClickApply);
        resetButton.onClick.AddListener(OnClickReset);
        closeButton.onClick.AddListener(OnClickClose);

        applyButton.gameObject.SetActive(false);

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
        if (_applyActionCount > 0)
        {
            _onApply?.Invoke();
            var handlers = _onApply?.GetInvocationList();
            if (handlers != null)
            {
                foreach (Action handler in handlers)
                    OnApply -= handler;
            }
        }
    }

    void OnClickReset()
    {
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
