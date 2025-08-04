using UnityEngine;

public enum UIType
{
    Scene,
    Popup,
}


public enum UIToggleState
{
    Off,
    On
}

public static class UIExtensions
{
    public static string ToCustomString(this UIToggleState toggleState)
    {
        return toggleState switch
        {
            UIToggleState.Off => "ui-off",
            UIToggleState.On => "ui-on",
            _ => "",
        };
    }
}
