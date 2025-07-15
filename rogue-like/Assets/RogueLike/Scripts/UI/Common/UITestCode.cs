using UnityEngine;

public class UITestCode : MonoBehaviour
{
    [SerializeField] RectTransform target;
    [SerializeField] RectTransform parent;

    [ContextMenu("Test")]
    public void Test()
    {
        target.IsFullyInsideRectTransform(parent);
    }
}
