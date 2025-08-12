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
    UITitleButton _selectedButton;

    private void Start()
    {
        newGameButton.Initialize(NewGame);
        continueButton.Initialize(Continue);
        loadButton.Initialize(Load);
        settinButton.Initialize(Setting);
        exitButton.Initialize(Exit);

        newGameButton.SetSoundSuppressed(true);
        EventSystem.current.SetSelectedGameObject(newGameButton.gameObject);
        UIManager.Instance.AddConfirmAction(Select);
    }

    private void OnDestroy()
    {
        if (UIManager.Instance == null) return;
        UIManager.Instance.DeleteConfirmAction(Select);
    }

    public void SetAction(UITitleButton titleButton, UnityAction action)
    {
        _selectedButton = titleButton;
        _onClickAction = action;
    }

    void Select(InputAction.CallbackContext context)
    {
        if (UIManager.Instance.IsHasPopupUI() == true) return;
        SoundExtensions.PlayUIButton();
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
        {
            _selectedButton.SetSoundSuppressed(true);
            EventSystem.current.SetSelectedGameObject(_selectedButton.gameObject);
        }

        Navigation navigation = newGameButton.navigation;
        navigation.mode = mode;

        newGameButton.navigation = navigation;
        continueButton.navigation = navigation;
        loadButton.navigation = navigation;
        settinButton.navigation = navigation;
        exitButton.navigation = navigation;
    }
}