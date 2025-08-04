using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UISettingPopup : UIPopup
{
    [Header("Buttons")]
    [SerializeField] Button _applyButton;
    [SerializeField] Button _resetButton;

    UISettingCotent[] _settingList;

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
        UIManager.Instance.AddApplyAction(Apply);
        UIManager.Instance.AddResetAction(ResetAction);

        _applyButton.gameObject.SetActive(false);
        _settingList = GetComponentsInChildren<UISettingCotent>(true);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (UIManager.Instance == null) return;
        UIManager.Instance.DeleteApplyAction(Apply);
        UIManager.Instance.DeleteResetAction(ResetAction);
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
        UICommonConfirmPopup confirmPopup = UIManager.Instance.OpenPopupUI<UICommonConfirmPopup>();
        confirmPopup.Initialize(() => {
            SoundManager.Instance.ResetOption();
            DisplayManager.Instance.ResetOption();
            foreach (var setting in _settingList)
            {
                setting.ResetOption();
            }
        },
        "설정 초기화",
        "현재 모든 설정을 기본값으로\n" +
        "초기화 하시겠습니까?",
        "초기화", "취소");
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

    public override void Close()
    {
        if (_applyActionCount == 0)
        {

        }
        else
        {
            UICommonConfirmPopup confirmPopup = UIManager.Instance.OpenPopupUI<UICommonConfirmPopup>();
            confirmPopup.Initialize(()=> { 
                OnClickApply(); 
            }, 
            "설정 변경 적용", 
            "적용하지 않은 설정 변경 사항이 있습니다.\n" +
            "적용하고 설정을 마치겠습니까?", 
            "적용", "취소");
        }
    }
}
