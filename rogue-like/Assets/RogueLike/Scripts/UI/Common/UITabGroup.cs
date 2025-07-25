using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UITabGroup : MonoBehaviour
{
    [Header("Tabs & Pages")]
    [SerializeField] List<UITabButton> _tabs = new();
    [SerializeField] List<GameObject> _pages = new();

    [Header("Input")]
    [SerializeField] InputActionReference prevAction;
    [SerializeField] InputActionReference nextAction;

    int _currentIndex;

    private void Awake()
    {
        for (int i = 0; i < _tabs.Count; i++)
        {
            _tabs[i].Index = i;
            _tabs[i].Group = this;
        }
    }

    private void OnEnable()
    {
        if (prevAction != null)
        {
            prevAction.action.performed += _ => Shift(-1);
            prevAction.action.Enable();
        }
        if (nextAction != null)
        {
            nextAction.action.performed += _ => Shift(+1);
            nextAction.action.Enable();
        }
        SetActiveTab(0);
    }

    private void OnDisable()
    {
        if (prevAction != null)
        {
            prevAction.action.performed -= _ => Shift(-1);
            prevAction.action.Disable();
        }
        if (nextAction != null)
        {
            nextAction.action.performed -= _ => Shift(+1);
            nextAction.action.Disable();
        }
    }

    void Shift(int dir)
    {
        int next = (_currentIndex + dir + _tabs.Count) % _tabs.Count;
        SetActiveTab(next);
    }

    public void SetActiveTab(int index)
    {
        if (index < 0 || index >= _tabs.Count) return;
        _currentIndex = index;

        for (int i = 0; i < _tabs.Count; i++)
        {
            bool on = (i == index);
            _tabs[i].SetSelected(on);
            if (i < _pages.Count) _pages[i].SetActive(on);
        }
    }
}
