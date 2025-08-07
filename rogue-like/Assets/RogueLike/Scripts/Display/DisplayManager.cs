using System.Collections;
using UnityEngine;

public class DisplayManager : Manager<DisplayManager>
{
    const string KEY_SCREENMODE = "Display.ScreenMode";
    const string KEY_RESOLUTION = "Display.Resolution";
    const string KEY_TARGETFRAMERATE = "Display.TargetFrameRate";
    const string KEY_LIMITFRAMERATE = "Display.LimitFrameRate";
    const string KEY_VSYNC = "Display.VSync";

    DisplayScreenMode _screenMode;
    DisplayResolution _resolution;
    DisplayTargetFrameRate _targetFrameRate;
    bool _limitFrameRate;
    bool _vSync;

    public DisplayScreenMode GetScreenMode() => _screenMode;
    public DisplayResolution GetResolution() => _resolution;
    public DisplayTargetFrameRate GetTargetFrameRate() => _targetFrameRate;
    public bool GetLimitFrameRate() => _limitFrameRate;
    public bool GetVSync() => _vSync;

    bool _isDisplayResoultionChanging = false;

    public override void Initialize()
    {
#if UNITY_STANDALONE
        _screenMode = (DisplayScreenMode)PlayerPrefs.GetInt(KEY_SCREENMODE, (int)DisplayScreenMode.FullScreenWindow);
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

    public void SetScreenMode(DisplayScreenMode screenMode)
    {
        _screenMode = screenMode;
        if (_isDisplayResoultionChanging == false)
            StartCoroutine(ApplyDisplayRoutine());
        SavePref(KEY_SCREENMODE, (int)_screenMode);
    }

    public void SetResolution(DisplayResolution resolution)
    {
        _resolution = resolution;
        if (_isDisplayResoultionChanging == false)
            StartCoroutine(ApplyDisplayRoutine());
        SavePref(KEY_RESOLUTION, (int)_resolution);
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

    void ApplyDisplay()
    {
        (int width, int height) = _resolution.ToSize();
        FullScreenMode fullScreenMode = _screenMode.ToFullScreenMode();
        Screen.SetResolution(width, height, fullScreenMode);
    }

    IEnumerator ApplyDisplayRoutine()
    {
        _isDisplayResoultionChanging = true;
        yield return new WaitForEndOfFrame();
        FullScreenMode fullScreenMode = _screenMode.ToFullScreenMode();
        Screen.fullScreenMode = fullScreenMode;
        yield return new WaitForEndOfFrame();
        (int width, int height) = _resolution.ToSize();
        Screen.SetResolution(width, height, fullScreenMode);
        _isDisplayResoultionChanging = false;
    }

    void ApplyTargetFrameRate()
    {
        Application.targetFrameRate = _limitFrameRate ? _targetFrameRate.ToInt() : -1;
    }

    void ApplyVSync()
    {
        QualitySettings.vSyncCount = _vSync ? 1 : 0;
    }
}
