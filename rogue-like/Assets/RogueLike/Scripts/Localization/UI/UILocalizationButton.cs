using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(UISettingButton))]
public class UILocalizationButton : MonoBehaviour
{
    UISettingButton _button;
    LocalizationLanguage[] _languages;

    private void Awake()
    {
        _languages = Enum.GetValues(typeof(LocalizationLanguage)).Cast<LocalizationLanguage>().ToArray();

        _button = GetComponent<UISettingButton>();
        _button.Initialize(_languages.Select(language => language.ToCustomString()).ToArray(),
            Array.IndexOf(_languages, LocalizationManager.Instance.GetLanguage()),
            OnValueChanged);
        _button.SetResetAction(ResetAction);
        _button.SetTitleLocalizationKey("ui-textLanguage");
    }

    void OnValueChanged(int value)
    {
        LocalizationManager.Instance.SetLanguage((LocalizationLanguage)value);
    }

    void ResetAction()
    {
        _button.SetIndexWithoutNotify(Array.IndexOf(_languages, LocalizationManager.Instance.GetLanguage()));
    }
}