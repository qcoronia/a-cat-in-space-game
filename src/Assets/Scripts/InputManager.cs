using System;
using System.Collections;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    public InputState inputState;
    public GameState gameState;

    void Start()
    {
        inputState.IsEnabled = true;
        inputState.EnableJoyStick = true;
        inputState.EnableButtons = false;
        inputState.BtnBack = false;
        inputState.JoyStick = new Joystick
        {
            initPos = new Vector2(Screen.width / 2, Screen.height / 2)
        };
    }

    void Update()
    {
        // App Back Button
        inputState.BtnBack = Input.GetKeyDown(KeyCode.Escape);

        inputState.HasInput = false;
        if (!inputState.IsEnabled)
        {
            return;
        }

        if (!gameState)
        {
            return;
        }

        if (gameState.state == GameplayState.EndingCutscene)
        {
            inputState.IsEnabled = false;
            return;
        }
        
//#if UNITY_EDITOR
//       if (Input.GetButtonDown("Fire1"))
//       {
//           if (inputState.EnableJoyStick)
//           {
//               inputState.JoyStick.initPos = Input.mousePosition;
//           }

//           if (inputState.EnableButtons)
//           {
//               inputState.BtnPrimary = true;
//           }
//       }
//       else if (Input.GetButton("Fire1"))
//       {
//           if (inputState.JoyStick.initPos != Input.mousePosition.ToVector2())
//           {
//               inputState.HasInput = true;
//           }
//           inputState.JoyStick.curPos = Input.mousePosition;

//           if (inputState.EnableButtons)
//           {
//               inputState.BtnPrimary = false;
//           }
//       }
//       else if (Input.GetButtonUp("Fire1"))
//       {
//           if (inputState.EnableButtons)
//           {
//               inputState.BtnPrimary = false;
//           }
//       }
//#elif ANDROID 
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            switch(touch.phase)
            {
                case TouchPhase.Began:
                    if (inputState.EnableJoyStick)
                    {
                        inputState.JoyStick.initPos = touch.position;
                    }

                    if (inputState.EnableButtons)
                    {
                        inputState.BtnPrimary = true;
                    }
                    break;
                case TouchPhase.Stationary:
                case TouchPhase.Moved:
                    if (inputState.JoyStick.initPos != touch.position)
                    {
                        inputState.HasInput = true;
                    }
                    inputState.JoyStick.curPos = touch.position;

                    if (inputState.EnableButtons)
                    {
                        inputState.BtnPrimary = false;
                    }
                    break;
                case TouchPhase.Ended:
                    if (inputState.EnableButtons)
                    {
                        inputState.BtnPrimary = false;
                    }
                    break;
                default:
                    break;
            }
        }
//#endif
    }
}
