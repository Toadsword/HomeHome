using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public static class GameInput {

    static bool logInput = true;

    static float deadZoneKeyboard = 0.1f;
    public static bool keyboardLastPressed = false;

    public enum InputType
    {
        LEFT,
        RIGHT,
        UP,
        DOWN,
        ACTION,
        MENU
    }
    public enum AxisType
    {
        HORIZONTAL,
        VERTICAL
    }
    public enum TimeType
    {
        UP,
        DOWN,
        HOLD
    }
    public enum DirectionType
    {
        MOUSE,
        INPUT
    }

    public static bool GetInputDown(InputType inputType)
    {
        return GetInput(inputType, TimeType.DOWN);
    }
    public static bool GetInputUp(InputType inputType)
    {
        return GetInput(inputType, TimeType.UP);
    }
    public static bool GetInput(InputType inputType)
    {
        return GetInput(inputType, TimeType.HOLD);
    }

    public static bool GetInput(InputType inputType, TimeType timeType)
    {
        InputDevice device = InputManager.ActiveDevice;
        bool result = false;
        switch (inputType)
        {
            case InputType.LEFT:
                result = GetInput(InputControlType.LeftStickLeft, timeType) ||
                            GetInput(KeyCode.D, timeType) ||
                            GetInput(KeyCode.RightArrow, timeType);
                break;
            case InputType.RIGHT:
                result = GetInput(InputControlType.LeftStickRight, timeType)||
                            GetInput(KeyCode.A, timeType) ||
                            GetInput(KeyCode.LeftArrow, timeType);
                break;
            case InputType.UP:
                result = GetInput(InputControlType.LeftStickUp, timeType) ||
                            GetInput(KeyCode.W, timeType) ||
                            GetInput(KeyCode.UpArrow, timeType);
                break;
            case InputType.DOWN:
                result = GetInput(InputControlType.LeftStickDown, timeType) ||
                            GetInput(KeyCode.S, timeType) ||
                            GetInput(KeyCode.DownArrow, timeType);
                break;
            case InputType.ACTION:
                result = GetInput(InputControlType.Action1, timeType) ||
                            GetInput(KeyCode.Space, timeType);
                break;
            case InputType.MENU:
                result = GetInput(InputControlType.Menu, timeType) ||
                            GetInput(KeyCode.Escape, timeType);
                break;
        }

        if (logInput && result)
            Debug.Log("Input : " + inputType.ToString() + " was pressed and called !");

        CheckLastControllerUsed();
        return result;
    }
    public static bool GetInput(InputControlType input, TimeType timeType)
    {
        InputDevice device = InputManager.ActiveDevice;
        switch (timeType)
        {
            case TimeType.UP:
                return device.GetControl(input).WasReleased;
            case TimeType.DOWN:
                return device.GetControl(input).WasPressed;
            case TimeType.HOLD:
                return device.GetControl(input).IsPressed;
        }
        return false;
    }
    public static bool GetInput(KeyCode keyCode, TimeType timeType)
    {
        switch (timeType)
        {
            case TimeType.UP:
                return Input.GetKeyUp(keyCode);
            case TimeType.DOWN:
                return Input.GetKeyDown(keyCode);
            case TimeType.HOLD:
                return Input.GetKey(keyCode);
        }
        return false;
    }
    public static bool GetInput(int mouseBtn, TimeType timeType)
    {
        switch (timeType)
        {
            case TimeType.UP:
                return Input.GetMouseButtonUp(mouseBtn);
            case TimeType.DOWN:
                return Input.GetMouseButtonDown(mouseBtn);
            case TimeType.HOLD:
                return Input.GetMouseButton(mouseBtn);
        }
        return false;
    }

    public static float GetAxis(AxisType axisType)
    {
        float deltaMove = 0.0f;
        InputDevice device = InputManager.ActiveDevice;
        switch (axisType)
        {
            case AxisType.HORIZONTAL:
                if (device.GetControl(InputControlType.LeftStickX).IsPressed)
                    deltaMove = device.GetControl(InputControlType.LeftStickX).Value;
                else if (device.GetControl(InputControlType.DPadX).IsPressed)
                    deltaMove = device.GetControl(InputControlType.DPadX).Value;
                else if(Mathf.Abs(Input.GetAxis("Horizontal")) > deadZoneKeyboard)
                    deltaMove = Input.GetAxis("Horizontal");
                break;
            case AxisType.VERTICAL:
                if (device.GetControl(InputControlType.LeftStickY).IsPressed)
                    deltaMove = device.GetControl(InputControlType.LeftStickY).Value;
                else if (device.GetControl(InputControlType.DPadY).IsPressed)
                    deltaMove = device.GetControl(InputControlType.DPadY).Value;
                else if(Mathf.Abs(Input.GetAxis("Vertical")) > deadZoneKeyboard)
                    deltaMove = Input.GetAxis("Vertical");
                break;
        }
        if (logInput && deltaMove != 0.0f)
            Debug.Log("Moving : " + axisType.ToString() + " " + deltaMove);

        CheckLastControllerUsed();
        return deltaMove;
    }
    public static float GetAxisRaw(AxisType axisType)
    {
        float deltaMove = 0.0f;
        InputDevice device = InputManager.ActiveDevice;
        switch (axisType)
        {
            case AxisType.HORIZONTAL:
                if (device.GetControl(InputControlType.LeftStickX).IsPressed)
                    deltaMove = device.GetControl(InputControlType.LeftStickX).RawValue;
                else if (device.GetControl(InputControlType.DPadX).IsPressed)
                    deltaMove = device.GetControl(InputControlType.DPadX).RawValue;
                else if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > deadZoneKeyboard)
                    deltaMove = Input.GetAxisRaw("Horizontal");
                break;
            case AxisType.VERTICAL:
                if (device.GetControl(InputControlType.LeftStickY).IsPressed)
                    deltaMove = device.GetControl(InputControlType.LeftStickY).RawValue;
                else if (device.GetControl(InputControlType.DPadY).IsPressed)
                    deltaMove = device.GetControl(InputControlType.DPadY).RawValue;
                else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) > deadZoneKeyboard)
                    deltaMove = Input.GetAxisRaw("Vertical");
                break;
        }
        if (logInput && deltaMove != 0.0f)
            Debug.Log("Moving : " + axisType.ToString() + " " + deltaMove);

        CheckLastControllerUsed();
        return deltaMove;
    }

    public static Vector2 GetDirection(DirectionType directionType, Vector2 origin)
    {
        Vector2 direction = new Vector2(0, 0);
        CheckLastControllerUsed();
        
        if(directionType == DirectionType.MOUSE && keyboardLastPressed)
        {
            Vector3 v3 = Input.mousePosition;
            v3.z = 10.0f;
            v3 = Camera.main.ScreenToWorldPoint(v3);

            direction = new Vector2(origin.x - v3.x, origin.y - v3.y);
        }
        else
        {
            direction.x = GetAxis(AxisType.HORIZONTAL);
            direction.y = GetAxis(AxisType.VERTICAL);
        }
        
        direction.Normalize();

        if (logInput && (direction.x != 0.0f || direction.y != 0.0f))
            Debug.Log("Moving : " + direction.ToString());

        return direction;
    }

    private static void CheckLastControllerUsed()
    {
        InputDevice device = InputManager.ActiveDevice;
        if (device.AnyButtonIsPressed)
        {
            keyboardLastPressed = false;
        }
        if (Input.anyKey)
        {
            keyboardLastPressed = true;
        }
    }
}
