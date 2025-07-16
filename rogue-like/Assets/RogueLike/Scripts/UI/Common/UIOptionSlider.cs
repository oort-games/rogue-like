using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem.UI;

public class UIOptionSlider : Selectable
{
    [SerializeField] ScrollRect _scrollRect;

    [SerializeField] TextMeshProUGUI _headerText;
    [SerializeField] Button _prevButton;
    [SerializeField] Button _nextButton;
    [SerializeField] Slider slider;
    [SerializeField] float step = 5f;
    [SerializeField] float step2 = 1f;

    [SerializeField] GameObject _selected;

    RectTransform _rectTransform;
    InputSystemUIInputModule sysModule;

    protected override void Awake()
    {
        base.Awake();

        _rectTransform = GetComponent<RectTransform>();
    }

    protected override void Start()
    {
        base.Start();
        sysModule = EventSystem.current.GetComponent<InputSystemUIInputModule>();
        _prevButton.onClick.AddListener(OnClickPrev);
        _nextButton.onClick.AddListener(OnClickNext);
        SetSelectedVisual(false);
        _headerText.text = Regex.Match(gameObject.name, @"\d+").Value;
    }

    void OnClickPrev()
    {
        PrevOption();
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    void OnClickNext()
    {
        NextOption();
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    void PrevOption()
    {
        slider.value -= step;
    }

    void NextOption()
    {
        slider.value += step;
    }

    public override void OnSelect(BaseEventData eventData)
    {
        SetSelectedVisual(true);
        _rectTransform.EnsureVisibleInScrollView(_scrollRect);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        StartCoroutine(DelayedDeselectCheck());
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public override void OnMove(AxisEventData eventData)
    {
        if (EventSystem.current.currentSelectedGameObject != gameObject)
            return;

        Debug.Log($"{sysModule.moveRepeatDelay},{sysModule.moveRepeatRate}");
        switch (eventData.moveDir)
        {
            case MoveDirection.Left:
                slider.value -= step2;
                break;
            case MoveDirection.Right:
                slider.value += step2;
                break;
            case MoveDirection.Up:
                Navigate(eventData, FindSelectableOnUp());
                break;
            case MoveDirection.Down:
                Navigate(eventData, FindSelectableOnDown());
                break;
        }

        sysModule.moveRepeatRate = 0.05f;
    }

    void Navigate(AxisEventData eventData, Selectable sel)
    {
        if (sel != null && sel.IsActive())
        {
            eventData.selectedObject = sel.gameObject;
        }
    }

    IEnumerator DelayedDeselectCheck()
    {
        yield return null;

        GameObject next = EventSystem.current.currentSelectedGameObject;
        if (next == null)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
            yield break;
        }
        SetSelectedVisual(false);
        sysModule.moveRepeatRate = 0.1f;
    }

    public void SetSelectedVisual(bool isSelected)
    {
        _selected.SetActive(isSelected);
        _prevButton.gameObject.SetActive(isSelected);
        _nextButton.gameObject.SetActive(isSelected);
    }
}
