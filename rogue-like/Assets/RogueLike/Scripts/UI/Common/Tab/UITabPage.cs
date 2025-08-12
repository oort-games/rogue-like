using UnityEngine;
using UnityEngine.EventSystems;

public class UITabPage : MonoBehaviour
{
    [SerializeField] GameObject _firstSelectObject;
    bool _isSoundSuppressed;

    public void SetSelected(bool isOn)
    {
        gameObject.SetActive(isOn);
        if (isOn)
        {
            if (_firstSelectObject.TryGetComponent<UIScrollViewContent>(out var viewContent))
            {
                viewContent.SetSoundSuppressed(true);
            }
            if (_isSoundSuppressed == true)
            {
                _isSoundSuppressed = false;
            }
            else
            {
                SoundExtensions.PlayUITab();
            }
            EventSystem.current.SetSelectedGameObject(_firstSelectObject);
        }
    }

    public void SetSoundSuppressed(bool isSuppressed)
    {
        _isSoundSuppressed = isSuppressed;
    }
}
