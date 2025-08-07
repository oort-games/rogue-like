using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UITitleButton : Selectable
{
    [SerializeField] GameObject _highlight;
    [SerializeField] Color _normalColor;
    [SerializeField] Color _selectedColor;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void OnMove(AxisEventData eventData)
    {
        Selectable selectable = null;
        switch (eventData.moveDir)
        {
            case MoveDirection.Up:
                selectable = FindSelectableOnUp();
                break;
            case MoveDirection.Down:
                selectable = FindSelectableOnDown();
                break;
        }
        if (selectable != null)
        {
            if (selectable.transform.parent != transform.parent) return;
            SetSelectedVisual(false);
            Navigate(eventData, selectable);
        }
    }

    void Navigate(AxisEventData eventData, Selectable sel)
    {
        if (sel != null && sel.IsActive())
            eventData.selectedObject = sel.gameObject;
    }

    void SetSelectedVisual(bool isSelected)
    {
        _highlight.SetActive(isSelected);
    }
}
