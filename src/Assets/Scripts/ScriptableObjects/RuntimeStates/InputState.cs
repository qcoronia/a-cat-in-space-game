using System;
using UnityEngine;

[CreateAssetMenu(
    fileName = "InputState",
    menuName = "Cat's Ship/Runtime States/Input State",
    order = 0)]
public class InputState : ScriptableObject
{
    public Joystick JoyStick;
    public bool HasInput;
    public bool IsEnabled;
    public bool EnableJoyStick;
    public bool EnableButtons;
    public bool BtnPrimary;
    public bool BtnSecondary;
    public bool BtnBack;
}

[Serializable]
public class Joystick
{
    public Vector2 initPos;
    public Vector2 curPos;
    public Vector2 value
    {
        get
        {
            return curPos - initPos;
        }
    }
}
