using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    // Reference to the generated Input Actions asset
    private PlayerInputActions _playerInputActions;

    // Public properties to expose input states
    public Vector2 LookInput { get; private set; }
    public bool FireInput { get; private set; }

    // Properties for single-frame button presses.
    // These are set to true when pressed and reset to false in LateUpdate.
    public bool FireInputThisFrame { get; private set; }
    public bool SwitchCharacterInputThisFrame { get; private set; }

    private void Awake()
    {
        // Initialize the Input Actions asset
        _playerInputActions = new PlayerInputActions();

        // Subscribe to input action events
        // Look input (continuous value)
        _playerInputActions.Gameplay.Look.performed += ProcessTouchLook;
        _playerInputActions.Gameplay.Look.canceled += ProcessTouchLook; // Reset when touch ends
    }

    private void OnEnable()
    {
        EnableGameplayInput();
    }

    private void OnDisable()
    {
        DisableGameplayInput();
    }

    /// <summary>
    /// Called after all Update calls. Used here to reset single-frame input flags.
    /// This ensures they are true for exactly one frame.
    /// </summary>
    private void LateUpdate()
    {
        // Reset single-frame input flags after they've been potentially read by other scripts
        FireInputThisFrame = false;
        SwitchCharacterInputThisFrame = false;
    }

    /// <summary>
    /// Enables the 'Gameplay' action map, allowing input events to be processed.
    /// </summary>
    public void EnableGameplayInput()
    {
        _playerInputActions.Gameplay.Enable();
    }

    /// <summary>
    /// Disables the 'Gameplay' action map, preventing input events from being processed.
    /// </summary>
    public void DisableGameplayInput()
    {
        _playerInputActions.Gameplay.Disable();
    }

    /// <summary>
    /// Callback for the 'Look' input action. Updates the LookInput vector.
    /// </summary>
    /// <param name="context">The context provided by the Input System.</param>
    public void ProcessTouchLook(InputAction.CallbackContext context)
    {
        // Read the Vector2 value (delta movement from touch)
        LookInput = context.ReadValue<Vector2>();
    }

    private void OnDestroy()
    {
        // Unsubscribe from input action events to prevent memory leaks
        _playerInputActions.Gameplay.Look.performed -= ProcessTouchLook;
        _playerInputActions.Gameplay.Look.canceled -= ProcessTouchLook;

        // Dispose the input actions asset
        _playerInputActions.Dispose();
    }
}
