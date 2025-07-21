using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UnityEngine.RectOffset를 최소화한 구조체
/// </summary>
[Serializable]
public struct RectTransformOffset
{
    public float top;
    public float bottom;
    public float left;
    public float right;
}

public static class RectTransformExtensions
{
    public static void EnsureVisibleInScrollView(this RectTransform rectTransform, ScrollRect scrollRect, RectTransformOffset margin = default)
    {
        if (rectTransform.IsFullyInsideRectTransform(scrollRect.viewport, margin)) return;
        rectTransform.ScrollIntoView(scrollRect, margin);
    }

    public static bool IsFullyInsideRectTransform(this RectTransform rectTransform, RectTransform rootRectTransform, RectTransformOffset margin = default)
    {
        Bounds bound = RectTransformUtility.CalculateRelativeRectTransformBounds(rootRectTransform, rectTransform);

        return bound.min.x >= rootRectTransform.rect.xMin && bound.max.x <= rootRectTransform.rect.xMax
            && bound.min.y >= rootRectTransform.rect.yMin && bound.max.y + margin.top <= rootRectTransform.rect.yMax;
    }

    /// <summary>
    /// 지정된 RectTransform을 ScrollRect 안에서 화면에 보이도록 스크롤합니다.
    /// 이 코드는 scroll Rect content의 pivot이 (x = 0, y = 1)으로 기준으로 작성되었습니다.
    /// </summary>
    public static void ScrollIntoView(this RectTransform rectTransform, ScrollRect scrollRect, RectTransformOffset margin = default)
    {
        RectTransform content = scrollRect.content;
        RectTransform viewport = scrollRect.viewport;

        Bounds bound = RectTransformUtility.CalculateRelativeRectTransformBounds(content, rectTransform);

        if (scrollRect.vertical)
        {
            float itemTop = -bound.max.y - margin.top;
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
