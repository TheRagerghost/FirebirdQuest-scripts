using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
using UnityEngine.UI;

public class ControlsManager : MonoBehaviour
{
    const string mouseScheme = "Keyboard&mouse";
    const string gamepadScheme = "Gamepad";
    string prevControlScheme = "";
    Mouse mainMouse;

    [Header("Controls")]

    public InputActionAsset controls;
    public PlayerInput playerInput;

    [HideInInspector]
    public Mouse currentMouse;
    

    InputActionMap actionMap;
    InputAction lookAction;

    [HideInInspector]
    public InputAction smartphoneToggle;

    [HideInInspector]
    public InputAction interact;

    [Header("Keyboard&mouse Settings")]

    public Texture2D cursorTexture;

    [Header("Gamepad Settings")]

    public RectTransform canvas;
    public RectTransform cursor;
    public float cursorSpeed = 1000f;
    public float padding = 35f;

    Mouse virtualMouse;
    bool prevMouseState;

    [Range(0.0f, 1.0f)]
    public float stickDeadZone = 0.15f;

    void Awake()
    {
        actionMap = controls.FindActionMap("Gameplay");
        lookAction = actionMap.FindAction("look");
        smartphoneToggle = actionMap.FindAction("togglePhone");
        interact = actionMap.FindAction("interact");

        lookAction.Enable();
        smartphoneToggle.Enable();
        interact.Enable();

        mainMouse = Mouse.current;
        currentMouse = mainMouse;
        Cursor.SetCursor(cursorTexture, new Vector2(26f, 18f), CursorMode.ForceSoftware);
    }

    private void OnEnable()
    {

        if (virtualMouse == null)
        {
            virtualMouse = (Mouse) InputSystem.AddDevice("VirtualMouse");
        } 
        else if (!virtualMouse.added)
        {
            InputSystem.AddDevice(virtualMouse);
        }

        InputUser.PerformPairingWithDevice(virtualMouse, playerInput.user);

        if (cursor != null)
        {
            Vector2 position = cursor.anchoredPosition;
            InputState.Change(virtualMouse.position, position);
        }

        InputSystem.onAfterUpdate += UpdateGamepadCursor;
        playerInput.onControlsChanged += OnControlsChanged;
    }

    private void OnDisable()
    {
        playerInput.user.UnpairDevice(virtualMouse);
        if (virtualMouse != null && virtualMouse.added) InputSystem.RemoveDevice(virtualMouse);
        InputSystem.onAfterUpdate -= UpdateGamepadCursor;
        playerInput.onControlsChanged -= OnControlsChanged;
    }

    void Update()
    {

    }

    void OnControlsChanged(PlayerInput input)
    {
        if (input.currentControlScheme == mouseScheme && prevControlScheme != mouseScheme)
        {
            currentMouse = mainMouse;
            cursor.GetComponent<Image>().enabled = false;
            //Cursor.visible = true;
            mainMouse.WarpCursorPosition(virtualMouse.position.ReadValue());
            prevControlScheme = mouseScheme;
        } 
        else if (input.currentControlScheme == gamepadScheme && prevControlScheme != gamepadScheme)
        {
            currentMouse = virtualMouse;
            cursor.GetComponent<Image>().enabled = true;
            //Cursor.visible = false;
            InputState.Change(virtualMouse.position, mainMouse.position.ReadValue());
            AnchorCursor(virtualMouse.position.ReadValue());
            prevControlScheme = gamepadScheme;
        }
    }

    public Vector2 GetInputLookRotation()
    {
        var delta = lookAction.ReadValue<Vector2>() * GameController.instance.lookSensitivity * Time.deltaTime;
        return delta;
    }

    public bool IsCameraRotationAllowed()
    {
        bool canRotate = Mouse.current != null ? Mouse.current.rightButton.isPressed : false;
        canRotate |= Gamepad.current != null ? Gamepad.current.rightStick.ReadValue().magnitude > stickDeadZone : false;
        return canRotate;
    }

    void UpdateGamepadCursor()
    {
        if (virtualMouse == null || Gamepad.current == null) return;

        Vector2 deltaValue = Gamepad.current.leftStick.ReadValue() * cursorSpeed * Time.deltaTime;
        Vector2 targetValue = virtualMouse.position.ReadValue() + deltaValue;

        targetValue.x = Mathf.Clamp(targetValue.x, padding, Screen.width - padding);
        targetValue.y = Mathf.Clamp(targetValue.y, padding, Screen.height - padding);

        InputState.Change(virtualMouse.position, targetValue);
        InputState.Change(virtualMouse.delta, deltaValue);

        bool isAButtonPressed = Gamepad.current.aButton.IsPressed();
        if (prevMouseState != isAButtonPressed)
        {
            virtualMouse.CopyState<MouseState>(out var mouseState);
            mouseState.WithButton(MouseButton.Left, Gamepad.current.aButton.isPressed);
            InputState.Change(virtualMouse, mouseState);
            prevMouseState = isAButtonPressed;
        }

        AnchorCursor(targetValue);
    }

    void AnchorCursor(Vector2 pos)
    {
        Vector2 anchoredPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, pos, null, out anchoredPosition);
        cursor.anchoredPosition = anchoredPosition;
    }

}
