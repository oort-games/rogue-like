using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public abstract class UIPopup : UIBase
{
    [Header("Common")]
    [SerializeField] Button _closeButton;

    protected virtual void Awake()
    {
        _type = UIType.Popup;
    }

    protected virtual void Start()
    {
        _closeButton.onClick.AddListener(Close);
    }

    protected virtual void OnDestroy()
    {
    }

    public virtual void Close()
    {
        UIManager.Instance.ClosePopupUI(this);
    }
}
