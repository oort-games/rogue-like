using UnityEngine;

public abstract class UIPopup : UIBase
{
    protected virtual void Awake()
    {
        _type = UIType.Popup;
    }

    public virtual void Close()
    {
        UIManager.Instance.ClosePopupUI(this);
    }
}
