using UnityEngine;

public class DisplayManager : Manager<DisplayManager>
{
    const string KEY_RESOLUTION = "Display.Resolution";
    const string KEY_MODE = "Display.Mode";

#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
    const FullScreenMode NativeFullScreen = FullScreenMode.MaximizedWindow;
#else
    const FullScreenMode NativeFullScreen = FullScreenMode.ExclusiveFullScreen;
#endif

    DisplayScreenMode _mode;
    DisplayResolution _resolution;

    public override void Initialize()
    {
#if UNITY_STANDALONE
        _mode = (DisplayScreenMode)PlayerPrefs.GetInt(KEY_MODE, (int)DisplayScreenMode.FullScreen);
        _resolution = (DisplayResolution)PlayerPrefs.GetInt(KEY_RESOLUTION, (int)DisplayResolution.FHD_1920x1080);
        ApplyDisplay();
#endif
    }

    public DisplayResolution GetResolution()
    {
        return _resolution;
    }

    public DisplayScreenMode GetMode()
    {
        return _mode;
    }

    public void SetResolution(DisplayResolution resolution)
    {
        _resolution = resolution;
        ApplyDisplay();
        SavePref(KEY_RESOLUTION, (int)_resolution);
    }

    public void SetMode(DisplayScreenMode mode)
    {
        _mode = mode;
        ApplyDisplay();
        SavePref(KEY_MODE, (int)mode);
    }

    public void ApplyDisplay()
    {
        Resolution resolution = _resolution.ToResolution();
        FullScreenMode mode = _mode switch
        {
            DisplayScreenMode.FullScreen => NativeFullScreen,
            DisplayScreenMode.FullScreenWindow => FullScreenMode.FullScreenWindow,
            DisplayScreenMode.Window => FullScreenMode.Windowed,
            _ => FullScreenMode.FullScreenWindow
        };
        Screen.SetResolution(resolution.width, resolution.height, mode);
    }

    void SavePref(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
    }
}
