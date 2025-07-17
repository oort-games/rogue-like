using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

public class InputManager : Manager<InputManager>
{
    [SerializeField] InputSystemUIInputModule _sysModule;

    public override void Initialize()
    {
        
    }

    public void SetMoveRepeatRate(float value)
    {
        _sysModule.moveRepeatRate = value;
    }
}
