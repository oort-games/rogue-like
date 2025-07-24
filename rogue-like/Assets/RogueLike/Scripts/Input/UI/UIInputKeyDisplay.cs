using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class UIInputKeyDisplay : MonoBehaviour
{
    [SerializeField] InputActionReference _inputActionRef;
    [SerializeField] TextMeshProUGUI _valueText;

    private void OnEnable()
    {
        UpdateText(InputManager.Instance.CurrentScheme);
        InputManager.Instance.OnSchemeChanged += (scheme) => UpdateText(scheme);
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.OnSchemeChanged -= (scheme) => UpdateText(scheme);
    }

    void UpdateText(string scheme)
    {
        InputAction action = _inputActionRef.action;

        int bindingIndex = FindBindingForScheme(action, scheme);

        _valueText.text = action.GetBindingDisplayString(
            bindingIndex,
            InputBinding.DisplayStringOptions.DontUseShortDisplayNames |
            InputBinding.DisplayStringOptions.IgnoreBindingOverrides);
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
