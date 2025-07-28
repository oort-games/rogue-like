using UnityEngine;
using TMPro;

[AddComponentMenu("Settings/UI/Display/UI Display TargetFrameRate Text")]
[RequireComponent(typeof(TextMeshProUGUI))]
public class UIDisplayTargetFrameRateText : MonoBehaviour
{
    TextMeshProUGUI _text;
    float _deltaTime;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
        float fps = 1f / _deltaTime;
        _text.text = $"FPS : {Mathf.RoundToInt(fps)}";
    }
}
