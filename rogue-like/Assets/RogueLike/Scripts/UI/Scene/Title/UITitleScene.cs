using UnityEngine;
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

    public void SetAction(UnityAction action)
    {
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
        Debug.Log("Setting");
        UIManager.Instance.OpenPopupUI<UISettingPopup>();
    }

    void Exit()
    {
        Debug.Log("Exit");
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
        "ui-ok", "ui-cancel");
    }
}