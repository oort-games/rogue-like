using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TMPro;

public class UISelectPopup : UIPopup
{
    [SerializeField] TextMeshProUGUI _titleText;
    [SerializeField] GameObject _contentPrefab;
    [SerializeField] Button _selectButton;

    string[] _options;
    int _currentIndex;
    UnityAction<int> _selectAction;

    public void Initialize(string titleLocalizationKey, string[] options, int selectIndex, UnityAction<int> selectAction)
    {
        _titleText.text = LocalizationManager.Instance.GetString(titleLocalizationKey);
        _options = options;
        _currentIndex = selectIndex;
        _selectAction = selectAction;

        _selectButton.onClick.AddListener(() => { _selectAction.Invoke(_currentIndex); });
        _selectButton.onClick.AddListener(Close);
    }

    protected override void Start()
    {
        base.Start();
        CreatContent();
        UIManager.Instance.AddConfirmAction(Select);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (UIManager.Instance == null) return;
        UIManager.Instance.DeleteConfirmAction(Select);
    }

    void CreatContent()
    {
        _contentPrefab.gameObject.SetActive(false);
        for (int i = 0; i < _options.Length; i++)
        {
            string option = _options[i];
            UISelectContent content = Instantiate(_contentPrefab, _contentPrefab.transform.parent).GetComponent<UISelectContent>();
            content.gameObject.SetActive(true);
            content.Initialize(i, option, _selectAction, SetIndex);

            if (i == _currentIndex)
            {
                EventSystem.current.SetSelectedGameObject(content.gameObject);
            }
        }
    }

    void SetIndex(int index)
    {
        _currentIndex = index;
    }

    void Select(InputAction.CallbackContext context)
    {
        if (UIManager.Instance.IsLastPopup(gameObject) == false) return;
        _selectAction.Invoke(_currentIndex);
        Close();
    }
}