using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[Serializable]
public class MoveInputEvent : UnityEvent<float, float> { }
[Serializable]
public class JumpInputEvent : UnityEvent<float> { }
[Serializable]
public class RunInputEvent : UnityEvent<float> { }
[Serializable]
public class CastSpellInputEvent : UnityEvent<float> { }
[Serializable]
public class SelectSpellInputEvent : UnityEvent<float> { }
[Serializable]
public class CancleSelectSpellInputEvent : UnityEvent<float> { }
[Serializable]
public class CancleSpellInputEvent : UnityEvent<float> { }

public class InputController : MonoBehaviour
{
    public static VeloxActions controls;
    public MoveInputEvent moveInputEvent;
    public JumpInputEvent jumpInputEvent;
    public RunInputEvent runInputEvent;
    public CastSpellInputEvent castSpellInputEvent;
    public SelectSpellInputEvent selectSpellInputEvent;
    public CancleSelectSpellInputEvent cancleSelectSpellInputEvent;
    public CancleSpellInputEvent cancleSpellInputEvent;


    private void Awake()
    {
        controls = new VeloxActions();
        switchCursorState(false, CursorLockMode.Locked);
    }

    private void OnEnable()
    {
        controls.Player.Enable();
        controls.Player.Move.performed += OnMovePerformed;
        controls.Player.Jump.performed += OnJumpPerformed;
        controls.Player.Run.performed += OnRunPerformed;
        controls.Player.Cast.performed += OnLeftClickPerformed;
        controls.Player.Select.started += OnRightClickPerformed;
        controls.Player.Cancel.started += OnCtrlPerformed;

        controls.Player.Move.canceled += OnMovePerformed;
        controls.Player.Jump.canceled += OnJumpPerformed;
        controls.Player.Run.canceled += OnRunPerformed;
        controls.Player.Select.canceled += OnRightClickCancled;
    }

    private void switchCursorState(bool visible, CursorLockMode lockMode)
    {
        Cursor.lockState = lockMode;
        Cursor.visible = visible;
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        float jumpInput = context.ReadValue<float>();
        jumpInputEvent.Invoke(jumpInput);
    }

    private void OnRunPerformed(InputAction.CallbackContext context)
    {
        float runInput = context.ReadValue<float>();
        runInputEvent.Invoke(runInput);
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        moveInputEvent.Invoke(moveInput.x, moveInput.y);
    }

    private void OnLeftClickPerformed(InputAction.CallbackContext context)
    {
        float selectInput = context.ReadValue<float>();
        castSpellInputEvent.Invoke(selectInput);
    }

    private void OnRightClickPerformed(InputAction.CallbackContext context)
    {
        float selectInput = context.ReadValue<float>();
        selectSpellInputEvent.Invoke(selectInput);
        switchCursorState(true, CursorLockMode.Confined);
    }

    private void OnRightClickCancled(InputAction.CallbackContext context)
    {
        float selectInput = context.ReadValue<float>();
        cancleSelectSpellInputEvent.Invoke(selectInput);
        switchCursorState(false, CursorLockMode.Locked);
    }

    private void OnCtrlPerformed(InputAction.CallbackContext context)
    {
        float selectInput = context.ReadValue<float>();
        cancleSpellInputEvent.Invoke(selectInput);
    }
}
