using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    public static PlayerInput _playerInput;

    public static float _moveHorizontal { get; set; }
    public static float _moveVertical { get; set; }

    public static bool _jumpDown { get; set; }

    public static bool _interactDown { get; set; }

    public static bool _whistle { get; set; }
    public static bool _whistleDown { get; set; }
    public static bool _whistleUp { get; set; }

    public static bool _crouch { get; set; }

    public static bool _menuDown { get; set; }
    public static bool _startUp { get; set; }

    public static bool _anyKeyDown { get; set; }
    public static bool _anyConDown { get; set; }

    private InputAction _moveHorAction;
    private InputAction _moveVerAction;

    private InputAction _jumpAction;
    private InputAction _interactAction;
    private InputAction _whistleAction;
    private InputAction _crouchAction;
    private InputAction _menuAction;
    private InputAction _startAction;

    private InputAction _anyKeyAction;
    private InputAction _anyConAction;

    void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();

        _moveHorAction = _playerInput.actions["Run"];
        _moveVerAction = _playerInput.actions["Ladder"];

        _jumpAction = _playerInput.actions["Jump"];
        _interactAction = _playerInput.actions["Interact"];
        _whistleAction = _playerInput.actions["Whistle"];
        _crouchAction = _playerInput.actions["Crouch"];
        _menuAction = _playerInput.actions["Menu"];
        _startAction = _playerInput.actions["StartGame"];

        _anyKeyAction = _playerInput.actions["AnyKeyboard"];
        _anyConAction = _playerInput.actions["AnyController"];
    }

    // Update is called once per frame
    void Update()
    {
        _moveHorizontal = _moveHorAction.ReadValue<float>();
        _moveVertical = _moveVerAction.ReadValue<float>();

        _jumpDown = _jumpAction.WasPressedThisFrame();

        _interactDown = _interactAction.WasPressedThisFrame();

        _whistle = _whistleAction.IsPressed();
        _whistleDown = _whistleAction.WasPressedThisFrame();
        _whistleUp = _whistleAction.WasReleasedThisFrame();

        _crouch = _crouchAction.IsPressed();

        _menuDown = _menuAction.WasPressedThisFrame();
        _startUp = _startAction.WasReleasedThisFrame();

        _anyKeyDown = _anyKeyAction.WasPressedThisFrame();
        _anyConDown = _anyConAction.WasPressedThisFrame();

    }
}
