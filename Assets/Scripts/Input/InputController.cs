using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Controller", menuName = "Controller/InputController")]
public class InputController : ScriptableObject, VeloxActions.IPlayerActions
{
    public event UnityAction<Vector2> moveInputEvent;
    public event UnityAction<float> jumpInputEvent;

    public event UnityAction<float> selectSpellInputEvent;
    public event UnityAction cancelSpellInputEvent;
    
    public SpellCastEvent spellCastEvent;
    private VeloxActions controls;

    private void OnEnable()
    {
        if (controls == null)
        {
            controls = new VeloxActions();
            controls.Player.SetCallbacks(this);
            switchCursorState(false, CursorLockMode.Locked);
        }
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void switchCursorState(bool visible, CursorLockMode lockMode)
    {
        Cursor.lockState = lockMode;
        Cursor.visible = visible;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (moveInputEvent != null && context.phase == InputActionPhase.Performed)
            moveInputEvent.Invoke(context.ReadValue<Vector2>());
        if (moveInputEvent != null && context.phase == InputActionPhase.Canceled)
            moveInputEvent.Invoke(context.ReadValue<Vector2>());
    }

    public void OnCast(InputAction.CallbackContext context)
    {
        if (spellCastEvent != null && context.phase == InputActionPhase.Performed)
        {
            spellCastEvent.Raise(new SpellCastInfo(Input.mousePosition));
        }
    }

    public void OnSelect(InputAction.CallbackContext context)
    {
        if (selectSpellInputEvent != null && context.phase == InputActionPhase.Performed) {
            selectSpellInputEvent.Invoke(context.ReadValue<float>());
        }
        if (selectSpellInputEvent != null && context.phase == InputActionPhase.Canceled) {
            selectSpellInputEvent.Invoke(context.ReadValue<float>());
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (jumpInputEvent != null && context.phase == InputActionPhase.Performed)
            jumpInputEvent.Invoke(context.ReadValue<float>());
        if (jumpInputEvent != null && context.phase == InputActionPhase.Canceled)
            jumpInputEvent.Invoke(context.ReadValue<float>());
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        if (cancelSpellInputEvent != null && context.phase == InputActionPhase.Performed)
            cancelSpellInputEvent.Invoke();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
    }
}
