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

        Bounds bound = RectTransformUtility.CalculateRelativeRectTransformBounds(viewport, rectTransform);

        return bound.min.x >= viewport.rect.xMin && bound.max.x <= viewport.rect.xMax
            && bound.min.y >= viewport.rect.yMin && bound.max.y <= viewport.rect.yMax;   
    }

    public static void ScrollIntoView(this RectTransform rectTransform, ScrollRect scrollRect)
    {
        RectTransform content = scrollRect.content;
        RectTransform viewport = scrollRect.viewport;

        Bounds bound = RectTransformUtility.CalculateRelativeRectTransformBounds(content, rectTransform);

        if (scrollRect.vertical)
        {
            float itemTop = -bound.max.y;   // content pivotY = 1 기준 (위→+)
            float itemBottom = -bound.min.y;
            float itemH = bound.size.y;

            float viewH = viewport.rect.height;
            float viewTop = content.anchoredPosition.y;
            float viewBottom = viewTop + viewH;

            float newViewTop = viewTop;

            if (itemH <= viewH)
            {
                if (itemTop < viewTop) newViewTop = itemTop;
                else if (itemBottom > viewBottom) newViewTop = itemBottom - viewH;
            }
            else
            {
                if (itemTop < viewTop || itemBottom > viewBottom)
                    newViewTop = itemTop;
            }

            float scrollableY = Mathf.Max(1f, content.rect.height - viewH);
            scrollRect.verticalNormalizedPosition = 1f - Mathf.Clamp01(newViewTop / scrollableY);
        }

        if (scrollRect.horizontal)
        {
            // content pivotX = 0 기준 (왼쪽→0, 오른쪽→+)
            float itemLeft = bound.min.x;
            float itemRight = bound.max.x;
            float itemW = bound.size.x;

            float viewW = viewport.rect.width;
            float viewLeft = -content.anchoredPosition.x;
            float viewRight = viewLeft + viewW;

            float newViewLeft = viewLeft;

            if (itemW <= viewW)
            {
                if (itemLeft < viewLeft) newViewLeft = itemLeft;
                else if (itemRight > viewRight) newViewLeft = itemRight - viewW;
            }
            else
            {
                if (itemLeft < viewLeft || itemRight > viewRight)
                    newViewLeft = itemLeft;
            }

            float scrollableX = Mathf.Max(1f, content.rect.width - viewW);
            scrollRect.horizontalNormalizedPosition = Mathf.Clamp01(newViewLeft / scrollableX);
        }

        Canvas.ForceUpdateCanvases();
    }
}
