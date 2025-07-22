using UnityEngine;
using UnityEngine.EventSystems;

public class UISetting : MonoBehaviour
{
    [SerializeField] GameObject _firstSelectObject;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(_firstSelectObject);
    }
}
