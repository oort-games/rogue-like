using UnityEngine;

public enum LocalizationLanguage
{
    Korean,
    English,
}

public static class LocalizationExtensions
{
    public static string ToCode(this LocalizationLanguage language)
    {
        return language switch
        {
            LocalizationLanguage.Korean => "ko-KR",
            LocalizationLanguage.English => "en",
            _ => "",
        };
    }

    public static string ToCustomString(this LocalizationLanguage language)
    {
        return language switch
        {
            LocalizationLanguage.Korean => "ÇÑ±¹¾î",
            LocalizationLanguage.English => "English",
            _ => "",
        };
    }
}