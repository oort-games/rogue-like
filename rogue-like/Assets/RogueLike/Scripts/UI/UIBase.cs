using UnityEngine;

[RequireComponent(typeof(Canvas))]
public abstract class UIBase: MonoBehaviour
{
    protected UIType _type;

    public int GetCanvasSortOrder()
    {
        return GetComponent<Canvas>().sortingOrder;
    }
    public void SetCanvasSortOrder(int order)
    {
        GetComponent<Canvas>().sortingOrder = order;
    }
}
