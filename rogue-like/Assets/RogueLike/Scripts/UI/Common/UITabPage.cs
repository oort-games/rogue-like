using UnityEngine;
using UnityEngine.EventSystems;

public class UITabPage : MonoBehaviour
{
    [SerializeField] GameObject _firstSelectObject;

    public void SetSelected(bool isOn)
    {
        gameObject.SetActive(isOn);
        if (isOn)
            EventSystem.current.SetSelectedGameObject(_firstSelectObject);
    }
}
