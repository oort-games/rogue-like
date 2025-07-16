using UnityEngine;
using UnityEngine.UI;

public class UITestButton : MonoBehaviour
{
    Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => Debug.Log("Click"));
    }
}
