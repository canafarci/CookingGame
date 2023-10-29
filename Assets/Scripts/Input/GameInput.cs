using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public enum Binding
    {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        Interact_Alternate,
        Pause,
        Gamepad_Interact,
        Gamepad_InteractAlternative,
        Gamepad_Pause
    }
    //singleton
    public static GameInput Instance { get; private set; }
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;
    public static PlayerInputActions _playerInputActions;
    //constants
    private const string BINDINGS_SAVE_KEY = "InpBin";
    //events
    public event EventHandler OnKeysBindingsRebound;
    private void Awake()
    {
        //init singleton
        if (Instance)
            Destroy(gameObject);
        else
            Instance = this;

        _playerInputActions = new PlayerInputActions();
        //load key bindings
        if (PlayerPrefs.HasKey(BINDINGS_SAVE_KEY))
        {
            _playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(BINDINGS_SAVE_KEY));
        }

        _playerInputActions.Player.Enable();
        _playerInputActions.Player.Interact.performed += InteractPerformed;
        _playerInputActions.Player.InteractAlternate.performed += InteractAlternatePerformed;
        _playerInputActions.Player.Pause.performed += PausePerformed;

    }

    private void PausePerformed(InputAction.CallbackContext context)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternatePerformed(InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractPerformed(InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetInputVectorNormalized()
    {
        Vector2 inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();

        return inputVector;
    }
    //Getters-Setters
    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            //indexes are relative to the order in input action mapping file
            case (Binding.Move_Up):
                return _playerInputActions.Player.Move.bindings[1].ToDisplayString();
            case (Binding.Move_Down):
                return _playerInputActions.Player.Move.bindings[2].ToDisplayString();
            case (Binding.Move_Left):
                return _playerInputActions.Player.Move.bindings[3].ToDisplayString();
            case (Binding.Move_Right):
                return _playerInputActions.Player.Move.bindings[4].ToDisplayString();
            case (Binding.Interact):
                return _playerInputActions.Player.Interact.bindings[0].ToDisplayString();
            case (Binding.Interact_Alternate):
                return _playerInputActions.Player.InteractAlternate.bindings[0].ToDisplayString();
            case (Binding.Pause):
                return _playerInputActions.Player.Pause.bindings[0].ToDisplayString();
            case (Binding.Gamepad_Interact):
                return _playerInputActions.Player.Interact.bindings[1].ToDisplayString();
            case (Binding.Gamepad_InteractAlternative):
                return _playerInputActions.Player.InteractAlternate.bindings[1].ToDisplayString();
            case (Binding.Gamepad_Pause):
                return _playerInputActions.Player.Pause.bindings[1].ToDisplayString();
            default:
                return null;
        }
    }
    public void RebindBinding(Binding binding, Action onBindingRebound)
    {
        InputAction inputAction;
        int bindingIndex;
        switch (binding)
        {
            //indexes are relative to the order in input action mapping file
            default:
            case (Binding.Move_Up):
                inputAction = _playerInputActions.Player.Move;
                bindingIndex = 1;
                break;
            case (Binding.Move_Down):
                inputAction = _playerInputActions.Player.Move;
                bindingIndex = 2;
                break;
            case (Binding.Move_Left):
                inputAction = _playerInputActions.Player.Move;
                bindingIndex = 3;
                break;
            case (Binding.Move_Right):
                inputAction = _playerInputActions.Player.Move;
                bindingIndex = 4;
                break;
            case (Binding.Interact):
                inputAction = _playerInputActions.Player.Interact;
                bindingIndex = 0;
                break;
            case (Binding.Gamepad_Interact):
                inputAction = _playerInputActions.Player.Interact;
                bindingIndex = 1;
                break;
            case (Binding.Interact_Alternate):
                inputAction = _playerInputActions.Player.InteractAlternate;
                bindingIndex = 0;
                break;
            case (Binding.Gamepad_InteractAlternative):
                inputAction = _playerInputActions.Player.InteractAlternate;
                bindingIndex = 1;
                break;
            case (Binding.Pause):
                inputAction = _playerInputActions.Player.Pause;
                bindingIndex = 0;
                break;
            case (Binding.Gamepad_Pause):
                inputAction = _playerInputActions.Player.Pause;
                bindingIndex = 1;
                break;
        }

        _playerInputActions.Player.Disable();
        inputAction.PerformInteractiveRebinding(bindingIndex).OnComplete((callback) =>
        {
            callback.Dispose();
            _playerInputActions.Player.Enable();
            onBindingRebound();
            string jsonText = _playerInputActions.SaveBindingOverridesAsJson();
            PlayerPrefs.SetString(BINDINGS_SAVE_KEY, jsonText);
            PlayerPrefs.Save();
            OnKeysBindingsRebound?.Invoke(this, EventArgs.Empty);
        })
        .Start();
    }
    //cleanup
    private void OnDestroy()
    {
        _playerInputActions.Player.Interact.performed -= InteractPerformed;
        _playerInputActions.Player.InteractAlternate.performed -= InteractAlternatePerformed;
        _playerInputActions.Player.Pause.performed -= PausePerformed;
        _playerInputActions.Dispose();
    }
}
