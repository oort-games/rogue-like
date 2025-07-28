using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UITabButton : MonoBehaviour
{
    [SerializeField] GameObject _highlight;

    Button _button;

    public int Index { get; set; }
    public UITabGroup Group { get; set; }

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() =>
        {
            Group.SetActiveTab(Index);
        });
    }

    public virtual void SetSelected(bool isOn) => _highlight.SetActive(isOn);
}
