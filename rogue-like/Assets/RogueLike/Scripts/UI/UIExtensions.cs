using UnityEngine;
using UnityEngine.UI;

public static class UIExtensions
{
    public static void EnsureVisibleInScrollView(this RectTransform rectTransform, ScrollRect scrollRect)
    {
        if (rectTransform.IsFullyInsideViewport(scrollRect)) return;
        rectTransform.ScrollIntoView(scrollRect);
    }

    public static bool IsFullyInsideViewport(this RectTransform rectTransform, ScrollRect scrollRect)
    {
        RectTransform viewport = scrollRect.viewport;

        Bounds b = RectTransformUtility.CalculateRelativeRectTransformBounds(viewport, rectTransform);

        return b.min.y >= viewport.rect.yMin && b.max.y <= viewport.rect.yMax;   
    }

    public static void ScrollIntoView(this RectTransform rectTransform, ScrollRect scrollRect)
    {
        RectTransform content = scrollRect.content;
        RectTransform viewport = scrollRect.viewport;

        Bounds b = RectTransformUtility.CalculateRelativeRectTransformBounds(content, rectTransform);

        float itemTop = -b.max.y;
        float itemBottom = -b.min.y;
        float itemHeight = b.size.y;

        float viewHeight = viewport.rect.height;
        float viewTop = content.anchoredPosition.y;
        float viewBottom = viewTop + viewHeight;

        float newViewTop = viewTop;

        if (itemHeight <= viewHeight)
        {
            if (itemTop < viewTop)
                newViewTop = itemTop;
            else if (itemBottom > viewBottom)
                newViewTop = itemBottom - viewHeight;
        }
        else
        {
            if (itemTop < viewTop || itemBottom > viewBottom)
                newViewTop = itemTop;
        }

        float contentScrollable = Mathf.Max(1f, content.rect.height - viewHeight);
        float normalized = 1f - Mathf.Clamp01(newViewTop / contentScrollable);

        scrollRect.verticalNormalizedPosition = normalized;
        Canvas.ForceUpdateCanvases();
    }
}
