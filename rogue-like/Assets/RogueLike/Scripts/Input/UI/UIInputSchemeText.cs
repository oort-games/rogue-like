using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UIInputSchemeText : MonoBehaviour
{
    TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

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
        _text.text = $"Scheme : {scheme}";
    }
}
