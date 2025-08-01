using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UISettingPopup : UIPopup
{
    [Header("Buttons")]
    [SerializeField] Button _applyButton;
    [SerializeField] Button _resetButton;

    [Header("Inputs")]
    [SerializeField] InputActionReference _applyActionRef;
    [SerializeField] InputActionReference _resetActionRef;

    UISettingBase[] _settingList;

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

    protected override void Start()
    {
        base.Start();

        _applyButton.onClick.AddListener(OnClickApply);
        _resetButton.onClick.AddListener(OnClickReset);

        _applyButton.gameObject.SetActive(false);

        _settingList = GetComponentsInChildren<UISettingBase>(true);
    }

    private void OnEnable()
    {
        _applyActionRef.action.performed += _ => OnClickApply();
        _applyActionRef.action.Enable();

        _resetActionRef.action.performed += _ => OnClickReset();
        _resetActionRef.action.Enable();
    }

    private void OnDisable()
    {
        _applyActionRef.action.performed -= _ => OnClickApply();
        _applyActionRef.action.Disable();

        _resetActionRef.action.performed -= _ => OnClickReset();
        _resetActionRef.action.Disable();
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

    public override void Close()
    {
        if (_applyActionCount == 0)
        {
            Debug.Log($"Setting Popup Close");
            //base.Close();
        }
        else
        {
            UICommonConfirmPopup confirmPopup = UIManager.Instance.OpenPopupUI<UICommonConfirmPopup>();
            confirmPopup.Initialize(()=> { OnClickApply(); confirmPopup.Close(); }, "타이틀", "정보", "확인", "취소");
        }
    }
}
