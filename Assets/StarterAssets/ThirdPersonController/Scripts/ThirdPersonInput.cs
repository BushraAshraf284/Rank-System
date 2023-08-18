using System.Collections;
using System.Collections.Generic;
using TouchControlsKit;

using UnityEngine;

public class ThirdPersonInput : MonoBehaviour
{
    public enum ControlTypes
    {
        PC,
        MOBILE
    }

    [Header("Controls")]
    public ControlTypes controlType;

    [Header("Character Input Values")]
    public Vector2 move;
    public Vector2 look;
    public bool jump;
    public bool sprint;
    public bool aim;

    [Header("Movement Settings")]
    public bool analogMovement;

    [Header("Mouse Cursor Settings")]
    public bool cursorLocked = true;
    public bool cursorInputForLook = true;

    [Header("Key Map")]
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode aimKey = KeyCode.Mouse1;

    void Update()
    {
#if UNITY_EDITOR
        if (controlType.Equals(ControlTypes.PC))
        {
            move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            look = new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
            sprint = Input.GetKey(sprintKey);
            jump = Input.GetKey(jumpKey);

            if (Input.GetKeyDown(aimKey))
                aim = !aim;
            if (sprint)
                aim = false;
        }
        else
        {
            move = TCKInput.GetAxis("Joystick");
            look = TCKInput.GetAxis("Touchpad");
            sprint = TCKInput.GetAction("Sprint", EActionEvent.Press);
            jump = TCKInput.GetAction("Jump", EActionEvent.Press);

            if (TCKInput.GetAction("Aim", EActionEvent.Down))
                aim = !aim;

            if (sprint)
                aim = false;
        }
#else
        move = TCKInput.GetAxis("Joystick");
        look = TCKInput.GetAxis("Touchpad");
        sprint = TCKInput.GetAction("Sprint", EActionEvent.Press);
        jump = TCKInput.GetAction("Jump", EActionEvent.Press);
        aim = TCKInput.GetAction("Aim", EActionEvent.Press);
#endif

    }

    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
