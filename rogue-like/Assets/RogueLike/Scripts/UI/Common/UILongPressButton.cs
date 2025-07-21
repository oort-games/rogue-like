using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UILongPressButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [SerializeField] float _longPressThreshold = 0.5f;
    [SerializeField] float _repeatRate = 0.1f;

    [Header("Events")]
    public UnityEvent onClick;
    public UnityEvent onLongPress;
    public UnityEvent onLongPressRepeat;

    Button _button;
    Coroutine _pressRoutine;
    bool _isPointerDown;
    bool _longPressFired;

    private void Awake()
    {
        _button = GetComponent<Button>();

        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() =>
        {
            if (!_longPressFired)
                onClick?.Invoke();
        });
    }

    public void SetSetRepeatRate(float value)
    {
        _repeatRate = value;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isPointerDown = true;
        _longPressFired = false;
        _pressRoutine = StartCoroutine(PressRoutine());
    }

    public void OnPointerUp(PointerEventData eventData) => CancelPress();

    public void OnPointerExit(PointerEventData eventData) => CancelPress();

    IEnumerator PressRoutine()
    {
        float timer = 0f;
        while (_isPointerDown && timer < _longPressThreshold)
        {
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        if (_isPointerDown)
        {
            _longPressFired = true;
            onLongPress?.Invoke();

            while (_isPointerDown)
            {
                yield return new WaitForSecondsRealtime(_repeatRate);
                onLongPressRepeat?.Invoke();
            }
        }
    }

    void CancelPress()
    {
        _isPointerDown = false;
        if (_pressRoutine != null)
        {
            StopCoroutine(_pressRoutine);
            _pressRoutine = null;
        }
    }
}
