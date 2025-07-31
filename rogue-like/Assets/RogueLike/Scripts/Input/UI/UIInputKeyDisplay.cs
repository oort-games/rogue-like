using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class UIInputKeyDisplay : MonoBehaviour
{
    [SerializeField] InputActionReference _inputActionRef;
    [SerializeField] TextMeshProUGUI _valueText;

    Dictionary<string, string> _symbols = new()
    {
        ["Triangle"] = "△",
        ["Circle"] = "○",
        ["Square"] = "□",
        ["Cross"] = "X",
    };

    private void OnEnable()
    {
        UpdateDisplay(InputManager.Instance.CurrentScheme);
        InputManager.Instance.OnSchemeChanged += (scheme) => UpdateDisplay(scheme);
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.OnSchemeChanged -= (scheme) => UpdateDisplay(scheme);
    }

    void UpdateDisplay(string scheme)
    {
        InputAction action = _inputActionRef.action;

        int bindingIndex = FindBindingForScheme(action, scheme);

        string displayStr = action.GetBindingDisplayString(
            bindingIndex,
            InputBinding.DisplayStringOptions.DontUseShortDisplayNames |
            InputBinding.DisplayStringOptions.IgnoreBindingOverrides);

        _valueText.text = _symbols.TryGetValue(displayStr, out var str) ? str : displayStr;
    }

    int FindBindingForScheme(InputAction action, string scheme)
    {
        for (int i = 0; i < action.bindings.Count; ++i)
        {
            var b = action.bindings[i];
            if (b.isComposite || b.isPartOfComposite) continue;

            // groups: "<Keyboard>&Mouse;Gamepad" 와 같은 문자열
            if (string.IsNullOrEmpty(scheme) || b.groups.Contains(scheme))
                return i;
        }
        return action.bindings.IndexOf(b => !b.isComposite && !b.isPartOfComposite);
    }
}
