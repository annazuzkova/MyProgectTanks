using System;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class InputManager : MonoBehaviour
{
    public static event Action OnSpacePressed;
    public static event Action OnLeftMouseButtonPressed;
    public static event Action OnFPressed;
    public static event Action<bool> OnShiftPressed;
    public static event Action<Vector2> OnMovementPressed;
    public static event Action<Vector2> OnLookPressed;


    public void OnSpacePressede(CallbackContext context)
    {
        if (context.performed)
        {
            OnSpacePressed?.Invoke();
        }
    }
    public void OnLeftMouseButtonPresse(CallbackContext context)
    {
        if (context.performed)
        {
            OnLeftMouseButtonPressed?.Invoke();
        }
    }
    public void OnFPresse(CallbackContext context)
    {
        if (context.started)
        {
            OnFPressed?.Invoke();
        }
    }

    public void OnMovePressede(CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 move = context.ReadValue<Vector2>();
            OnMovementPressed?.Invoke(move);
        }
        if (context.canceled)
        {
            OnMovementPressed?.Invoke(Vector2.zero);
        }
    }
    public void OnLookPressede(CallbackContext context)
    {
        Debug.Log("LOOK METHOD WORKS");

        if (context.performed)
        {
            Vector2 look = context.ReadValue<Vector2>();
            OnLookPressed?.Invoke(look);
        }

        if (context.canceled)
        {
            OnLookPressed?.Invoke(Vector2.zero);
        }
    }
    public void OnShiftPressede(CallbackContext context)
    {
        if (context.started)
        {
            OnShiftPressed?.Invoke(true);
        }
        else if (context.canceled)
        {
            OnShiftPressed?.Invoke(false);
        }
    }
}
