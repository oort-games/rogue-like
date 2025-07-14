using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;
using System.Text.RegularExpressions;

public class UIOptionSelector : Selectable
{
    [SerializeField] ScrollRect _scrollRect;

    [SerializeField] TextMeshProUGUI _headerText;
    [SerializeField] TextMeshProUGUI _valueText;
    [SerializeField] Button _prevButton;
    [SerializeField] Button _nextButton;
    [SerializeField] string[] _options;

    [SerializeField] GameObject _selected;

    [SerializeField] InputActionReference _navigateAction;

    RectTransform _rectTransform;
    int currentIndex = 0;

    public string GetCurrentOption() => _options[currentIndex];

    protected override void Awake()
    {
        base.Awake();

        _rectTransform = GetComponent<RectTransform>();
        _prevButton.onClick.AddListener(PrevOption);
        _nextButton.onClick.AddListener(NextOption);
        UpdateUI();
        _selected.SetActive(false);
        _headerText.text = Regex.Match(gameObject.name, @"\d+").Value;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _navigateAction.action.performed += OnNavigate;
        _navigateAction.action.Enable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _navigateAction.action.performed -= OnNavigate;
        _navigateAction.action.Disable();
    }

    void OnNavigate(InputAction.CallbackContext context)
    {
        //if (EventSystem.current.currentSelectedGameObject != gameObject)
        //    return;

        Vector2 input = context.ReadValue<Vector2>();

        if (input.x < -0.5f)
            PrevOption();
        else if (input.x > 0.5f)
            NextOption();
    }

    void PrevOption()
    {
        currentIndex = (currentIndex - 1 + _options.Length) % _options.Length;
        UpdateUI();
    }

    void NextOption()
    {
        currentIndex = (currentIndex + 1) % _options.Length;
        UpdateUI();
    }

    void UpdateUI()
    {
        _valueText.text = _options[currentIndex];
    }

    public override void OnSelect(BaseEventData eventData)
    {
        _selected.SetActive(true);
        _rectTransform.EnsureVisibleInScrollView(_scrollRect);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        _selected.SetActive(false);
    }
}
