using UnityEngine;

public enum DisplayResolution
{
    HD_1280x720,
    FHD_1920x1080,
}

public enum DisplayScreenMode
{
    FullScreen,
    FullScreenWindow,
    Window,
}

public static class DisplayExtensions
{
    public static Resolution ToResolution(this DisplayResolution preset) => preset switch
    {
        DisplayResolution.HD_1280x720 => new Resolution { width = 1280, height = 720},
        DisplayResolution.FHD_1920x1080 => new Resolution { width = 1920, height = 1080 },
        _ => new Resolution { width = 1920, height = 1080 }
    };
}