using System;
using UnityEngine;

public enum DisplayResolution
{
    Resolution_1280x720,
    Resolution_1360x768,
    Resolution_1366x768,
    Resolution_1600x900,
    Resolution_1920x1080,
}

public enum DisplayScreenMode
{
    FullScreen,
    FullScreenWindow,
    Window,
}

public enum DisplayTargetFrameRate
{
    FPS_30,
    FPS_45,
    FPS_60,
    FPS_75,
    FPS_90,
    FPS_120,
}

public static class DisplayExtensions
{
#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
    const FullScreenMode NativeFullScreen = FullScreenMode.MaximizedWindow;
#else
    const FullScreenMode NativeFullScreen = FullScreenMode.ExclusiveFullScreen;
#endif

    public static (int width, int height) ToSize(this DisplayResolution resolution) => resolution switch
    {
        DisplayResolution.Resolution_1280x720 => (1280, 720),
        DisplayResolution.Resolution_1360x768 => (1360, 768),
        DisplayResolution.Resolution_1366x768 => (1366, 768),
        DisplayResolution.Resolution_1600x900 => (1600, 900),
        DisplayResolution.Resolution_1920x1080 => (1920, 1080),
        _ => (1920, 1080)
    };

    public static FullScreenMode ToFullScreenMode(this DisplayScreenMode screenMode) => screenMode switch
    {
        DisplayScreenMode.FullScreen => NativeFullScreen,
        DisplayScreenMode.FullScreenWindow => FullScreenMode.FullScreenWindow,
        DisplayScreenMode.Window => FullScreenMode.Windowed,
        _ => FullScreenMode.FullScreenWindow
    };

    public static int ToInt(this DisplayTargetFrameRate targetFrameRate) => targetFrameRate switch
    {
        DisplayTargetFrameRate.FPS_30 => 30,
        DisplayTargetFrameRate.FPS_45 => 45,
        DisplayTargetFrameRate.FPS_60 => 60,
        DisplayTargetFrameRate.FPS_75 => 75,
        DisplayTargetFrameRate.FPS_90 => 90,
        DisplayTargetFrameRate.FPS_120 => 120,
        _ => -1
    };

    public static string ToCustomString(this DisplayResolution resolution)
    {
        (int width, int height) = resolution.ToSize();
        return $"{width} x {height}";
    }

    public static string ToCustomString(this DisplayScreenMode screenMode)
    {
        return screenMode switch
        {
            DisplayScreenMode.FullScreen => "ui-display-fullScreen",
            DisplayScreenMode.FullScreenWindow => "ui-display-fullScreenWindow",
            DisplayScreenMode.Window => "ui-display-window",
            _ => "",
        };
    }

    public static string ToCustomString(this DisplayTargetFrameRate targetFrameRate) => $"{targetFrameRate.ToInt()}";
}