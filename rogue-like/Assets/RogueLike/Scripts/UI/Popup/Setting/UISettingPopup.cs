using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using TMPro;

public class UISettingPopup : UIPopup
{
    [Header("Buttons")]
    [SerializeField] Button _applyButton;
    [SerializeField] Button _resetButton;

    [Header("Info")]
    [SerializeField] LocalizeStringEvent _infoTitleStringEvent;
    [SerializeField] LocalizeStringEvent _infoTextStringEvent;

    UISettingContent[] _settingList;
    UISettingContent _selectedContent;

    int _applyActionCount = 0;
    event Action _onApply;
    public event Action OnApply
    {
        add
        {
            _onApply += value;
            _applyActionCount++;
            _applyButton.gameObject.SetActive(_applyActionCount > 0);
        }
        remove
        {
            _onApply -= value;
            _applyActionCount--;
            _applyButton.gameObject.SetActive(_applyActionCount > 0);
        }
    }

    UnityAction _closeAction;

    protected override void Start()
    {
        base.Start();

        _applyButton.onClick.AddListener(OnClickApply);
        _resetButton.onClick.AddListener(OnClickReset);
        UIManager.Instance.AddApplyAction(Apply);
        UIManager.Instance.AddResetAction(ResetAction);

        _applyButton.gameObject.SetActive(false);
        _settingList = GetComponentsInChildren<UISettingContent>(true);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (UIManager.Instance == null) return;
        UIManager.Instance.DeleteApplyAction(Apply);
        UIManager.Instance.DeleteResetAction(ResetAction);
    }

    public override void Close()
    {
        if (_applyActionCount == 0)
        {
            base.Close();
            SoundExtensions.PlayUIButton();
            _closeAction?.Invoke();
        }
        else
        {
            UIConfirmPopup confirmPopup = UIManager.Instance.OpenPopupUI<UIConfirmPopup>();
            confirmPopup.Initialize(() => {
                OnClickApply();
            },
            "ui-applyChanges",
            "ui-applyChangesInfo",
            "ui-apply", "ui-cancel");
        }
    }
    
    public void SetCloseAction(UnityAction closeAction)
    {
        _closeAction = closeAction;
    }

    public void Select(UISettingContent content)
    {
        _selectedContent = content;
        _infoTitleStringEvent.StringReference = _selectedContent.GetTitleLocalizedString();
        _infoTextStringEvent.StringReference = _selectedContent.GetInfoLocalizedString();
    }

    void OnClickApply()
    {
        if (_applyActionCount > 0)
        {
            if (UIManager.Instance.GetPopupUI<UIConfirmPopup>() == null)
                SoundExtensions.PlayUIButton();
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
        SoundExtensions.PlayUIButton();
        UIConfirmPopup confirmPopup = UIManager.Instance.OpenPopupUI<UIConfirmPopup>();
        confirmPopup.Initialize(() => {
            SoundManager.Instance.ResetOption();
            DisplayManager.Instance.ResetOption();
            LocalizationManager.Instance.ResetOption();
            foreach (var setting in _settingList)
            {
                setting.ResetOption();
            }
        },
        "ui-resetSettings",
        "ui-resetSettingsInfo",
        "ui-reset", "ui-cancel");
    }

    void Apply(InputAction.CallbackContext context)
    {
        if (UIManager.Instance.IsLastPopup(gameObject) == false) return;
        OnClickApply();
    }

    void ResetAction(InputAction.CallbackContext context)
    {
        if (UIManager.Instance.IsLastPopup(gameObject) == false) return;
        OnClickReset();
    }
}
