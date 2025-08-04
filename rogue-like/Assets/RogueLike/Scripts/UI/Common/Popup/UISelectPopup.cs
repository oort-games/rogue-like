using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UISelectPopup : UIPopup
{
    [SerializeField] GameObject _contentPrefab;

    string[] _options;
    int _currentIndex;
    UnityAction<int> _confirmAction;

    public void Initialize(UnityAction<int> confirmAction, string[] options, int selectIndex)
    {
        _confirmAction = confirmAction;
        _options = options;
        _currentIndex = selectIndex;
    }

    protected override void Start()
    {
        base.Start();
        CreatContent();
    }

    void CreatContent()
    {
        _contentPrefab.gameObject.SetActive(false);
        for (int i = 0; i < _options.Length; i++)
        {
            string option = _options[i];
            UISelectContent content = Instantiate(_contentPrefab, _contentPrefab.transform.parent).GetComponent<UISelectContent>();
            content.gameObject.SetActive(true);
            content.Initialize(i, option, _confirmAction);

            if (i == _currentIndex)
            {
                EventSystem.current.SetSelectedGameObject(content.gameObject);
            }
        }
    }
}