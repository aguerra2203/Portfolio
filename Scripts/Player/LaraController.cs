using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LaraController : MonoBehaviour
{

    private PlayerInputActions playerInputActions;
    [SerializeField] private LaraScript laraScript;


    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        laraScript = GameObject.Find("Lara").GetComponent<LaraScript>();
    }
    // Start is called before the first frame update
    void Start()
    {
        GameManager._instance.OnGamePaused.AddListener(OnPauseReceived);
        GameManager._instance.OnGameResumed.AddListener(OnResumeRecevied);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        SubscribeInputActions();
        SwitchActionMap("Player");
    }

    private void OnDisable()
    {
        UnsubscribeInputActions();
        SwitchActionMap("None");
    }

    public void SwitchActionMap(string mapName)
    {

        switch (mapName)
        {
            case "Player":
                playerInputActions.UI.Disable();
                playerInputActions.Player.Enable();

                //Hide and lock the cursor
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;

            case "UI":
                playerInputActions.Player.Disable();
                playerInputActions.UI.Enable();

                //Show and release the cursor
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                break;

            case "None":
                playerInputActions.Player.Disable();
                playerInputActions.UI.Disable();
                break;


            default:
                Debug.Log("Unrecognized action map specified");
                break;

        }
    }

    private void OnPauseReceived()
    {
        SwitchActionMap("UI");
    }

    private void OnResumeRecevied()
    {
        SwitchActionMap("Player");
    }

    private void SubscribeInputActions()
    {
        //Called when input is registered
        playerInputActions.Player.Move.started += MoveAction;
        //Called when input is completed
        playerInputActions.Player.Move.performed += MoveAction;
        //Called when input is canceled before completion
        playerInputActions.Player.Move.canceled += MoveAction;

        playerInputActions.Player.Jump.performed += JumpAction;
        playerInputActions.Player.Jump.canceled += JumpAction;

        playerInputActions.Player.PauseGame.performed += PauseActionPerformed;

        playerInputActions.UI.PauseGame.performed += PauseActionPerformed;

    }

    private void UnsubscribeInputActions()
    {
        //Called when input is registered
        playerInputActions.Player.Move.started -= MoveAction;
        //Called when input is completed
        playerInputActions.Player.Move.performed -= MoveAction;
        //Called when input is canceled before completion
        playerInputActions.Player.Move.canceled -= MoveAction;

        playerInputActions.Player.Jump.performed -= JumpAction;
        playerInputActions.Player.Jump.canceled -= JumpAction;

        playerInputActions.Player.PauseGame.performed -= PauseActionPerformed;

        playerInputActions.UI.PauseGame.performed -= PauseActionPerformed;

    }

    private void MoveAction(InputAction.CallbackContext context)
    {
        laraScript.SetMovementInput(context.ReadValue<Vector2>());
    }

    private void JumpAction(InputAction.CallbackContext context)
    {
        laraScript.Jump();
    }

    private void JumpActionCanceled(InputAction.CallbackContext context)
    {
        laraScript.CancelJump();
    }
    private void PauseActionPerformed(InputAction.CallbackContext context)
    {
        GameManager._instance.TogglePause();
    }
}
