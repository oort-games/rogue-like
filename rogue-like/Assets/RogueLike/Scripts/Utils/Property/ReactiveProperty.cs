using UnityEngine.Events;

public class ReactiveProperty<T>
{
    T _value;

    public UnityAction<T> onValueChanged;

    public ReactiveProperty(T initialValue = default)
    {
        _value = initialValue;
    }

    public T Value
    {
        get => _value;
        set
        {
            if (Equals(_value, value)) return;

            _value = value;
            onValueChanged?.Invoke(_value);
        }
    }

    public void Subscribe(UnityAction<T> listener)
    {
        onValueChanged += listener;
        listener(_value);
    }

    public void Unsubscribe(UnityAction<T> listener)
    {
        onValueChanged -= listener;
    }
}
