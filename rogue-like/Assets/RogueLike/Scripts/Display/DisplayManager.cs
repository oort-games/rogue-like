using UnityEngine;

public class DisplayManager : Manager<DisplayManager>
{
    const string KEY_RESOLUTION = "Display.Resolution";
    const string KEY_SCREENMODE = "Display.ScreenMode";
    const string KEY_TARGETFRAMERATE = "Display.TargetFrameRate";

    DisplayScreenMode _screenMode;
    DisplayResolution _resolution;
    DisplayTargetFrameRate _targetFrameRate;

    public DisplayResolution GetResolution() => _resolution;
    public DisplayScreenMode GetScreenMode() => _screenMode;
    public DisplayTargetFrameRate GetTargetFrameRate() => _targetFrameRate;

    public override void Initialize()
    {
#if UNITY_STANDALONE
        _screenMode = (DisplayScreenMode)PlayerPrefs.GetInt(KEY_SCREENMODE, (int)DisplayScreenMode.FullScreen);
        _resolution = (DisplayResolution)PlayerPrefs.GetInt(KEY_RESOLUTION, (int)DisplayResolution.Resolution_1920x1080);
        _targetFrameRate = (DisplayTargetFrameRate)PlayerPrefs.GetInt(KEY_TARGETFRAMERATE, (int)DisplayTargetFrameRate.Auto);
        
        ApplyDisplay();
        ApplyTargetFrameRate();
#endif
    }

    public void SetResolution(DisplayResolution resolution)
    {
        _resolution = resolution;
        ApplyDisplay();
        SavePref(KEY_RESOLUTION, (int)_resolution);
    }

    public void SetScreenMode(DisplayScreenMode screenMode)
    {
        _screenMode = screenMode;
        ApplyDisplay();
        SavePref(KEY_SCREENMODE, (int)screenMode);
    }

    public void SetTargetFrameRate(DisplayTargetFrameRate targetFrameRate)
    {
        _targetFrameRate = targetFrameRate;
        ApplyTargetFrameRate();
        SavePref(KEY_TARGETFRAMERATE, (int)targetFrameRate);
    }

    public void ApplyDisplay()
    {
        (int width, int height) = _resolution.ToSize();
        FullScreenMode fullScreenMode = _screenMode.ToFullScreenMode();
        Screen.SetResolution(width, height, fullScreenMode);
    }

    public void ApplyTargetFrameRate()
    {
        Application.targetFrameRate = _targetFrameRate.ToInt();
    }

    void SavePref(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
    }
}
