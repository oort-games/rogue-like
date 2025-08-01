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
        _closeButton.onClick.AddListener(OnClickClose);
    }

    public virtual void Close()
    {
        UIManager.Instance.ClosePopupUI(this);
    }

    void OnClickClose()
    {
        if (UIManager.Instance.IsLastPopup(gameObject))
            Close();
    }
}
