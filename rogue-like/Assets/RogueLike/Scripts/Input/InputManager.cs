using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class InputManager : Manager<InputManager>
{
    [SerializeField] PlayerInput _playerInput;
    [SerializeField] InputSystemUIInputModule _sysModule;

    public string CurrentScheme => _playerInput.currentControlScheme;
    public event Action<string> OnSchemeChanged;

    public override void Initialize()
    {
    }

    public void SetMoveRepeatRate(float value) => _sysModule.moveRepeatRate = value;

    #region Input Action Events
    void OnControlsChanged(PlayerInput playerInput)
    {
        Debug.Log(_playerInput.currentControlScheme);
        OnSchemeChanged?.Invoke(CurrentScheme);
    }
    #endregion
}
