using UnityEngine;
using UnityEngine.InputSystem;

public static class DanceInput
{
    private static InputAction DanceInputActionLeft;
    private static InputAction DanceInputActionRight;
    private static InputAction DanceInputActionUp;
    private static InputAction DanceInputActionDown;

    public static InputDir GetInputPressed()
    {
        DanceInputActionLeft ??= InputSystem.actions.FindAction("DanceInputLeft");
        DanceInputActionRight ??= InputSystem.actions.FindAction("DanceInputRight");
        DanceInputActionUp ??= InputSystem.actions.FindAction("DanceInputUp");
        DanceInputActionDown ??= InputSystem.actions.FindAction("DanceInputDown");

        InputDir dir = InputDir.None;

        if (DanceInputActionLeft.WasPressedThisFrame())
            dir |= InputDir.Left;
        if (DanceInputActionRight.WasPressedThisFrame())
            dir |= InputDir.Right;
        if (DanceInputActionUp.WasPressedThisFrame())
            dir |= InputDir.Up;
        if (DanceInputActionDown.WasPressedThisFrame())
            dir |= InputDir.Down;
        
        return dir;
    }

    public static InputDir GetInputHeld()
    {
        DanceInputActionLeft ??= InputSystem.actions.FindAction("DanceInputLeft");
        DanceInputActionRight ??= InputSystem.actions.FindAction("DanceInputRight");
        DanceInputActionUp ??= InputSystem.actions.FindAction("DanceInputUp");
        DanceInputActionDown ??= InputSystem.actions.FindAction("DanceInputDown");

        InputDir dir = InputDir.None;

        if (DanceInputActionLeft.IsPressed())
            dir |= InputDir.Left;
        if (DanceInputActionRight.IsPressed())
            dir |= InputDir.Right;
        if (DanceInputActionUp.IsPressed())
            dir |= InputDir.Up;
        if (DanceInputActionDown.IsPressed())
            dir |= InputDir.Down;
        
        return dir;
    }

}
