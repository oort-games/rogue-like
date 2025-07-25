using UnityEngine;
using UnityEngine.EventSystems;

public class UISettingPage : MonoBehaviour
{
    [SerializeField] GameObject _firstSelectObject;
    bool _isStart = false;

    private void Start()
    {
        _isStart = true;
        EventSystem.current.SetSelectedGameObject(_firstSelectObject);
    }

    private void OnEnable()
    {
        if (!_isStart) return;
        EventSystem.current.SetSelectedGameObject(_firstSelectObject);
    }
}
