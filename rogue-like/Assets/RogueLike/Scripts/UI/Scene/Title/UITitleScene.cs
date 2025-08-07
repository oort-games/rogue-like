using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UITitleScene : UIScene
{
    [SerializeField] UITitleButton newGameButton;
    [SerializeField] UITitleButton continueButton;
    [SerializeField] UITitleButton loadButton;
    [SerializeField] UITitleButton settinButton;
    [SerializeField] UITitleButton exitButton;

    UnityAction _onClickAction;
    GameObject _selectedObject;

    private void Start()
    {
        newGameButton.Initialize(NewGame);
        continueButton.Initialize(Continue);
        loadButton.Initialize(Load);
        settinButton.Initialize(Setting);
        exitButton.Initialize(Exit);

        EventSystem.current.SetSelectedGameObject(newGameButton.gameObject);
        UIManager.Instance.AddConfirmAction(Select);
    }

    private void OnDestroy()
    {
        if (UIManager.Instance == null) return;
        UIManager.Instance.DeleteConfirmAction(Select);
    }

    public void SetAction(GameObject gameObject, UnityAction action)
    {
        _selectedObject = gameObject;
        _onClickAction = action;
    }

    void Select(InputAction.CallbackContext context)
    {
        if (UIManager.Instance.IsHasPopupUI() == true) return;
        _onClickAction.Invoke();
    }

    void NewGame()
    {
        Debug.Log("New Game");
    }

    void Continue()
    {
        Debug.Log("Continue");
    }

    void Load()
    {
        Debug.Log("Load");
    }

    void Setting()
    {
        SetNavigationMode(Navigation.Mode.None);
        UISettingPopup settingPopup = UIManager.Instance.OpenPopupUI<UISettingPopup>();
        settingPopup.SetCloseAction(() => SetNavigationMode(Navigation.Mode.Vertical));
    }

    void Exit()
    {
        SetNavigationMode(Navigation.Mode.None);
        UIConfirmPopup confirmPopup = UIManager.Instance.OpenPopupUI<UIConfirmPopup>();
        confirmPopup.Initialize(() => 
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        },
        "ui-exitGame",
        "ui-exitGameInfo",
        "ui-ok", "ui-cancel",
        () => SetNavigationMode(Navigation.Mode.Vertical));
    }

    void SetNavigationMode(Navigation.Mode mode)
    {
        if (mode == Navigation.Mode.None)
            EventSystem.current.SetSelectedGameObject(null);
        else
            EventSystem.current.SetSelectedGameObject(_selectedObject);

        Navigation navigation = newGameButton.navigation;
        navigation.mode = mode;

        newGameButton.navigation = navigation;
        continueButton.navigation = navigation;
        loadButton.navigation = navigation;
        settinButton.navigation = navigation;
        exitButton.navigation = navigation;
    }
}