using UnityEngine;

public abstract class UIScene : UIBase
{
    protected virtual void Awake()
    {
        _type = UIType.Scene;
        SetCanvasSortOrder(-1);
        UIManager.Instance.SetSceneUI(this);
    }
}
