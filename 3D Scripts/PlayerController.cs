using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This class collects all input from the player and communicates
/// it to the PlayerBody (CharacterBody) that belongs to the player.
/// </summary>

public class PlayerController : MonoBehaviour
{ 
    #region Variables

    [Header("Player Input")]
    private PlayerInputActions playerInputActions;

    [Header("Object/Component References")]
    [SerializeField] PlayerBody playerBody;

    #endregion

    #region Unity Functions

    private void Awake()
    {
        // Set up our player actions object in code
        playerInputActions = new PlayerInputActions(); // This class name is based on what you named your .inputactions asset
    }

    private void OnEnable()
    {
        // Here we can subscribe functions to our
        // input actions to make code occur when
        // our input actions occur
        SubscribeInputActions();

        // We need to enable the action map whose
        // actions we want to be listening for.
        // In this case, that's the player action map
        SwitchActionMap("Player");
    }

    private void OnDisable()
    {
        // Here we can unsubscribe our functions
        // from our input actions so our object
        // doesn't try to call functions after
        // it is destroyed
        UnSubscribeInputActions();

        // Disable our action maps
        SwitchActionMap("None");
    }

    #endregion

    #region Custom Functions

    #region Player Input

    private void SubscribeInputActions()
    {
        // Here we can bind our input actions to functions
        #region Player
        playerInputActions.Player.Move.started += MoveActionPerformed;
        playerInputActions.Player.Move.performed += MoveActionPerformed;
        playerInputActions.Player.Move.canceled += MoveActionPerformed;

        playerInputActions.Player.Jump.performed += JumpActionPerformed;
        playerInputActions.Player.Jump.canceled += JumpActionCanceled;

        playerInputActions.Player.Sprint.performed += SprintActionPerformed;
        playerInputActions.Player.Sprint.canceled += SprintActionCanceled;

        playerInputActions.Player.Flashlight.performed += FlashlightActionPerformed;
        playerInputActions.Player.CameraSwitch.performed += CameraSwitchActionPerformed;

        //playerInputActions.Player.Interact.performed += InteractActionPerformed;
        #endregion
    }

    private void UnSubscribeInputActions()
    {
        // It is important to unbind any actions that we bind
        // when our object is destroyed, or this can cause issues
        #region Player
        playerInputActions.Player.Move.started -= MoveActionPerformed;
        playerInputActions.Player.Move.performed -= MoveActionPerformed;
        playerInputActions.Player.Move.canceled -= MoveActionPerformed;

        playerInputActions.Player.Jump.performed -= JumpActionPerformed;
        playerInputActions.Player.Jump.canceled -= JumpActionCanceled;

        playerInputActions.Player.Sprint.performed -= SprintActionPerformed;
        playerInputActions.Player.Sprint.canceled -= SprintActionCanceled;

        playerInputActions.Player.Flashlight.performed -= FlashlightActionPerformed;
        playerInputActions.Player.CameraSwitch.performed += CameraSwitchActionPerformed;

        //playerInputActions.Player.Interact.performed -= InteractActionPerformed;
        #endregion
    }

    /// <summary>
    /// Helper function to switch to a particular action map
    /// in our player's Input Actions Asset.
    /// </summary>
    /// <param name="mapName">The name of the map we want to switch to.</param>
    private void SwitchActionMap(string mapName)
    {
        playerInputActions.Player.Disable();
        playerInputActions.UI.Disable();

        switch (mapName)
        {
            default:
            case "Player":
                // We need to enable our "Player" action map so Unity will listen for our player input.
                playerInputActions.Player.Enable();

                // Since we are switching into gameplay, we will
                // no longer need control of our mouse cursor
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;

            case "UI":
                // We need to enable our "UI" action map so Unity will listen for our UI input.
                playerInputActions.UI.Enable();

                // Since we are switching into a UI, we will also
                // need control of our mouse cursor
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                break;

            case "None":
                // Show the mouse cursor
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                break;
        }
    }

    #endregion

    #region Input Actions

    private void MoveActionPerformed(InputAction.CallbackContext context)
    {
        // Read in the Vector2 of our player input.
        Vector2 movementInput = context.ReadValue<Vector2>();

        playerBody.SetMoveInput(movementInput);
    }

    private void JumpActionPerformed(InputAction.CallbackContext context)
    {
        playerBody.Jump();
    }

    private void JumpActionCanceled(InputAction.CallbackContext context)
    {
        playerBody.JumpCanceled();
    }

    private void SprintActionPerformed(InputAction.CallbackContext context)
    {
        playerBody.StartSprinting();
    }

    private void SprintActionCanceled(InputAction.CallbackContext context)
    {
        playerBody.StopSprinting();
    }

    private void FlashlightActionPerformed(InputAction.CallbackContext context)
    {
       playerBody.FlashlightToggle();
    }

    private void CameraSwitchActionPerformed(InputAction.CallbackContext context)
    {
        playerBody.CameraSwitch();
    }

    //private void InteractActionPerformed(InputAction.CallbackContext context)
    //{
    //    playerBody.InteractActionPerformed(context);
    //}

    #endregion

    #endregion
}