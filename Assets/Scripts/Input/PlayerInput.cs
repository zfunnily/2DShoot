using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Player Input")]
public class PlayerInput : ScriptableObject, InputActions.IGameplayActions
{
    public event UnityAction<Vector2> onMove = delegate{};
    public event UnityAction onStopMove = delegate{};

    InputActions inputActions;
    void OnEnable()
    {
        inputActions = new InputActions();
        inputActions.Gameplay.SetCallbacks(this);
    }

    private void OnDisable()
    {
        DisableAllInputs();
    }
    public void DisableAllInputs()
    {
        inputActions.Gameplay.Disable();
    }

    public void EnableGameplayInput()
    {
        inputActions.Gameplay.Enable();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            onMove.Invoke(context.ReadValue<Vector2>());
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            onStopMove.Invoke();
        }
    }
}
