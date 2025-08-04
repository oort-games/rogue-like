using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LocalizationManager : Manager<LocalizationManager>
{
    bool _isChanging;

    public override void Initialize()
    {

    }

    public void ChangeLocalization(int index)
    {
        if (_isChanging) return;
        StartCoroutine(ChangeRoutine(index));
    }

    IEnumerator ChangeRoutine(int index)
    {
        _isChanging = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];

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