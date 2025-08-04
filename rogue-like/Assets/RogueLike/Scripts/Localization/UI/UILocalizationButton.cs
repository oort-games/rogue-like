using UnityEngine;

[RequireComponent(typeof(UISettingButton))]
public class UILocalizationButton : MonoBehaviour
{
    UISettingButton _button;

    private void Awake()
    {
        _button = GetComponent<UISettingButton>();
        _button.SetOption(LocalizationManager.Instance.GetLocales());
        _button.SetIndex(0);
        _button.SetAction(LocalizationManager.Instance.ChangeLocalization);
        _button.SetTitleLocalizationKey("ui-textLanguage");
    }
}
