using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LocalizationManager : Manager<LocalizationManager>
{
    const string KEY_LANGUAGE = "Localization.Language";

    LocalizationLanguage _language;

    public LocalizationLanguage GetLanguage() => _language;

    bool _isChanging;

    public override void Initialize()
    {
        _language = (LocalizationLanguage)PlayerPrefs.GetInt(KEY_LANGUAGE, (int)LocalizationLanguage.Korean);
        ApplyLanguage();
    }

    public void ResetOption()
    {
        SetLanguage(LocalizationLanguage.Korean);
    }

    public void SetLanguage(LocalizationLanguage language)
    {
        _language = language;
        ApplyLanguage();
        SavePref(KEY_LANGUAGE, (int)_language);
    }

    void ApplyLanguage()
    {
        if (_isChanging) return;
        StartCoroutine(ApplyLanguageRoutine());
    }

    IEnumerator ApplyLanguageRoutine()
    {
        _isChanging = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(_language.ToCode());

        _isChanging = false;
    }

    public string GetString(string key)
    {
        return LocalizationSettings.StringDatabase.GetLocalizedString(key);
    }

    public string GetString(string table, string key)
    {
        return LocalizationSettings.StringDatabase.GetLocalizedString(table, key);
    }
}