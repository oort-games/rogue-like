using System.Collections.Generic;
using UnityEngine;

public class UIManager : Manager<UIManager>
{
    UIScene _scene;
    List<UIPopup> _popupList = new();

    public override void Initialize()
    {

    }

    GameObject CreateUI(UIType type, string prefabName)
    {
        GameObject prefab = GetPrefab(type, prefabName);
        GameObject ui = Instantiate(prefab);
        ui.name = prefabName;
        return ui;
    }

    GameObject GetPrefab(UIType type, string prefabName)
    {
        return Resources.Load<GameObject>($"UI/{type}/{prefabName}");
    }

    public void SetSceneUI(UIScene scene)
    {
        if (_scene == null)
        {
            _scene = scene;
        }
        else
        {
            if (_scene != scene)
            {
                Destroy(_scene.gameObject);
                _scene = scene;
            }
        }
        _scene.transform.SetParent(transform);
    }

    public T OpenSceneUI<T>() where T : UIScene
    {
        T scene;
        if (_scene == null)
        {
            scene = CreateUI(UIType.Scene, typeof(T).Name).GetComponent<T>();
        }
        else
        {
            if (_scene.name == typeof(T).Name)
            {
                scene = _scene.GetComponent<T>();
            }
            else
            {
                scene = CreateUI(UIType.Scene, typeof(T).Name).GetComponent<T>();
            }
        }
        scene.gameObject.SetActive(true);

        return scene;
    }

    public T GetSceneUI<T>() where T : UIScene
    {
        if (_scene == null)
        {
            return null;
        }

        return _scene.GetComponent<T>();
    }

    public void CloseSceneUI()
    {
        if (_scene != null)
        {
            Destroy(_scene.gameObject);
            _scene = null;
        }
    }

    public T OpenPopupUI<T>() where T : UIPopup
    {
        T popup = GetPopupUI<T>();
        if (popup == null)
        {
            popup = CreateUI(UIType.Popup, typeof(T).Name).GetComponent<T>();
            popup.SetCanvasSortOrder(_popupList.Count);
            popup.transform.SetParent(transform);

            _popupList.Add(popup);
        }
        popup.gameObject.SetActive(true);

        return popup;
    }

    public T GetPopupUI<T>() where T : UIPopup
    {
        for (int i = 0; i < _popupList.Count; i++)
        {
            if (_popupList[i].name == typeof(T).Name)
            {
                return _popupList[i].GetComponent<T>();
            }
        }
        return null;
    }

    public void ClosePopupUI<T>() where T : UIPopup
    {
        T popup = GetPopupUI<T>();

        if (popup != null)
        {
            ClosePopupUI(popup);
        }
    }

    public void ClosePopupUI(UIPopup popup)
    {
        int index = popup.GetCanvasSortOrder();

        _popupList.Remove(popup);
        Destroy(popup.gameObject);

        for (int i = index; i < _popupList.Count; i++)
        {
            _popupList[i].SetCanvasSortOrder(i);
        }
    }
}
