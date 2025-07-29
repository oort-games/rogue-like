using UnityEngine;

public class DisplayManager : Manager<DisplayManager>
{
    const string KEY_RESOLUTION = "Display.Resolution";
    const string KEY_SCREENMODE = "Display.ScreenMode";
    const string KEY_TARGETFRAMERATE = "Display.TargetFrameRate";
    const string KEY_LIMITFRAMERATE = "Display.LimitFrameRate";
    const string KEY_VSYNC = "Display.VSync";

    DisplayScreenMode _screenMode;
    DisplayResolution _resolution;
    DisplayTargetFrameRate _targetFrameRate;
    bool _limitFrameRate;
    bool _vSync;

    public DisplayResolution GetResolution() => _resolution;
    public DisplayScreenMode GetScreenMode() => _screenMode;
    public DisplayTargetFrameRate GetTargetFrameRate() => _targetFrameRate;
    public bool GetLimitFrameRate() => _limitFrameRate;
    public bool GetVSync() => _vSync;

    public override void Initialize()
    {
#if UNITY_STANDALONE
        _screenMode = (DisplayScreenMode)PlayerPrefs.GetInt(KEY_SCREENMODE, (int)DisplayScreenMode.FullScreen);
        _resolution = (DisplayResolution)PlayerPrefs.GetInt(KEY_RESOLUTION, (int)DisplayResolution.Resolution_1920x1080);
        _targetFrameRate = (DisplayTargetFrameRate)PlayerPrefs.GetInt(KEY_TARGETFRAMERATE, (int)DisplayTargetFrameRate.FPS_120);
        _limitFrameRate = PlayerPrefs.GetInt(KEY_LIMITFRAMERATE, 1) == 1;
        _vSync = PlayerPrefs.GetInt(KEY_VSYNC, 0) == 1;

        ApplyDisplay();
        ApplyTargetFrameRate();
        ApplyVSync();
#endif
    }

    public void ResetOption()
    {
        SetScreenMode(DisplayScreenMode.FullScreenWindow);
        SetResolution(DisplayResolution.Resolution_1920x1080);
        SetTargetFrameRate(DisplayTargetFrameRate.FPS_120);
        SetLimitFrameRate(true);
        SetVSync(false);
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
        SavePref(KEY_SCREENMODE, (int)_screenMode);
    }

    public void SetTargetFrameRate(DisplayTargetFrameRate targetFrameRate)
    {
        _targetFrameRate = targetFrameRate;
        ApplyTargetFrameRate();
        SavePref(KEY_TARGETFRAMERATE, (int)_targetFrameRate);
    }

    public void SetLimitFrameRate(bool value)
    {
        _limitFrameRate = value;
        ApplyTargetFrameRate();
        SavePref(KEY_LIMITFRAMERATE, _limitFrameRate ? 1 : 0);
    }

    public void SetVSync(bool value)
    {
        _vSync = value;
        ApplyVSync();
        SavePref(KEY_VSYNC, _vSync ? 1 : 0);
    }

    public void ApplyDisplay()
    {
        (int width, int height) = _resolution.ToSize();
        FullScreenMode fullScreenMode = _screenMode.ToFullScreenMode();
        Screen.SetResolution(width, height, fullScreenMode);
    }

    public void ApplyTargetFrameRate()
    {
        Application.targetFrameRate = _limitFrameRate ? _targetFrameRate.ToInt() : -1;
    }

    public void ApplyVSync()
    {
        QualitySettings.vSyncCount = _vSync ? 1 : 0;
    }

    void SavePref(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
    }
}
