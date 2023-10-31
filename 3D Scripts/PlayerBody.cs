using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBody : CharacterBody
{
    #region variables
    [Space(20)]

    [Header("Player - Ground Movement")]
    [SerializeField] private float moveAcceleration = 60f;
    [SerializeField] private float walkSpeed = 7f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float maxVerticalMoveSpeed = 25f;
    private bool isSprinting = false;
    private Vector3 currentVelocity;

    [Header("Player - Air Movement")]
    [SerializeField] private int maxJumps = 2;
    private int currentJump = 0;
    [SerializeField] private float jumpCooldown = 0.25f;
    [SerializeField] private float airControlMultiplier = 0.4f;
    private bool readyToJump = true;

    [Header("Player - Rotation")]
    [SerializeField] private float playerGroundRotationSpeed = 10f;
    [SerializeField] private float playerAirRotationSpeed = 3f;

    [Header("Player - Ground Check")]
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private LayerMask environmentLayerMask;
    private bool isGrounded = true;
    private bool wasGroundedLastFrame;

    [Header("Player - Input")]
    private Vector2 movementInput;

    [Header("Camera")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform cameraOrientation;


    [Header("Component/Object References")]
    [SerializeField] private Light flashlight;
    [SerializeField] private CinemachineFreeLook playerCamera;


    public enum MovementState
    {
        Walking,
        Sprinting,
        Airborne
    }

    private MovementState currentMovementState = MovementState.Walking;
    #endregion

    #region Unity Functions

    private void Update()
    {

        //If camera changes
        CalculateCameraRelativeInput();

        //Detect any state changes and handle them
        StateHandler();

        RotateCharacter();

        animator.SetFloat("HorizontalSpeed", GetHorizontalRBVelocity().magnitude);
        animator.SetBool("isFalling", (characterRigidbody.velocity.y < 0));
    }
    private void FixedUpdate()
    {
        CheckGrounded();

        MoveCharacter();

        LimitVelocity();
    }
    #endregion

    #region Input
    public override void SetMoveInput(Vector2 moveInput)
    {
        movementInput = moveInput;

        //Any time our input changes, calculate camera relative input
        CalculateCameraRelativeInput();
    }

    void CalculateCameraRelativeInput()
    {
        if (movementInput == Vector2.zero)
        {
            movementDirection = Vector3.zero;
            return;
        }

        movementDirection = cameraOrientation.forward * movementInput.y + cameraOrientation.right * movementInput.x;

        //Cap movement at 1, keep smaller values
        if (movementDirection.sqrMagnitude > 1f)
        {
            movementDirection = movementDirection.normalized;
        }
    }

    #endregion

    #region Movement
    protected override void MoveCharacter()
    {
        if (movementDirection != Vector3.zero)
        {
            //On the ground
            if (isGrounded)
            {
                characterRigidbody.AddForce(movementDirection * moveAcceleration * characterRigidbody.mass, ForceMode.Force);
            }
            else //In the air
            {
                characterRigidbody.AddForce(movementDirection * moveAcceleration * characterRigidbody.mass * airControlMultiplier,
                ForceMode.Force);
            }
        }
    }

    private void LimitVelocity()
    {
        //Limit horizontal velocity
        currentVelocity = GetHorizontalRBVelocity();
        float maxAllowedVelocity = GetMaxAllowedVelocity();
        if (currentVelocity.sqrMagnitude > (maxAllowedVelocity * maxAllowedVelocity))
        {
            Vector3 counteractDirection = currentVelocity.normalized * -1f;
            float counteractAmount = currentVelocity.magnitude - maxAllowedVelocity;
            characterRigidbody.AddForce(counteractDirection * counteractAmount * characterRigidbody.mass, ForceMode.Impulse);
        }

        //Limit vertical velocity
        if (!isGrounded)
        {
            if (Mathf.Abs(characterRigidbody.velocity.y) > maxVerticalMoveSpeed)
            {
                Vector3 counteractDirection = Vector3.up * Mathf.Sign(characterRigidbody.velocity.y) * -1f;
                float counteractAmount = Mathf.Abs(characterRigidbody.velocity.y) - maxVerticalMoveSpeed;

                characterRigidbody.AddForce(counteractDirection * counteractAmount * characterRigidbody.mass,
                                            ForceMode.Impulse);
            }
        }

    }

    protected override void RotateCharacter()
    {
        Vector3 basicViewDir = transform.position - cameraTransform.position;
        basicViewDir.y = 0f;
        cameraOrientation.forward = basicViewDir.normalized;

        if (movementDirection != Vector3.zero)
        {
            characterModel.forward = Vector3.Slerp(characterModel.forward, movementDirection.normalized,
                                                   rotationSpeed * Time.deltaTime);
        }
    }

    public override void Jump()
    {
        if (readyToJump && (isGrounded || currentJump < maxJumps))
        {

            animator.SetTrigger("Jump");

            currentJump += 1;

            Vector3 counteractDirection = Vector3.up * Mathf.Sign(characterRigidbody.velocity.y) * -1f;
            float counteractAmount = Mathf.Abs(characterRigidbody.velocity.y);
            characterRigidbody.AddForce(counteractDirection * counteractAmount * characterRigidbody.mass, ForceMode.Impulse);

            characterRigidbody.AddForce(Vector3.up * jumpForce * characterRigidbody.mass, ForceMode.Impulse);

            readyToJump = false;
            StartCoroutine(JumpCooldownCoroutine());
        }
    }

    private IEnumerator JumpCooldownCoroutine()
    {
        yield return new WaitForSeconds(jumpCooldown);
        readyToJump = true;
    }

    public override void JumpCanceled()
    {
        if (characterRigidbody.velocity.y > 0f)
        {
            characterRigidbody.AddForce(Vector3.down * (characterRigidbody.velocity.y / 2f) * characterRigidbody.mass,
                                        ForceMode.Impulse);
        }
    }

    public void StartSprinting()
    {
        isSprinting = true;
    }

    public void StopSprinting()
    {
        isSprinting = false;
    }
    #endregion

    #region State Handling

    private void StateHandler()
    {
        //Figure out if we should switch states
        MovementState targetState = MovementState.Walking;

        if (!isGrounded)
        {
            targetState = MovementState.Airborne;
        }
        else if (isSprinting)
        {
            targetState = MovementState.Sprinting;
        }
        else
        {
            targetState = MovementState.Walking;
        }

        //If we are already in the state, return
        if (targetState == currentMovementState) return;

        //If we need to switch, what should we do
        currentMovementState = targetState;

        switch (currentMovementState)
        {
            case MovementState.Walking:
                moveSpeed = walkSpeed;
                rotationSpeed = playerGroundRotationSpeed;
                break;

            case MovementState.Sprinting:
                moveSpeed = sprintSpeed;
                rotationSpeed = playerGroundRotationSpeed;
                break;

            case MovementState.Airborne:
                rotationSpeed = playerAirRotationSpeed;
                break;

            default:
                Debug.LogError("Unrecognized movement state given!");
                break;
        }
    }

    #endregion

    #region Ground Checking

    private void CheckGrounded()
    {
        wasGroundedLastFrame = isGrounded;

        Vector3 overlapSphereOrigin = transform.position + (Vector3.up * (capsuleCollider.radius - groundCheckDistance));

        Collider[] overlappedColliders = Physics.OverlapSphere(overlapSphereOrigin, capsuleCollider.radius * 0.95f,
                                                               environmentLayerMask);

        isGrounded = (overlappedColliders.Length > 0);

        //We became grounded this frame
        if (!wasGroundedLastFrame && isGrounded)
        {
            currentJump = 0;
        }

        //We became airborne this frame
        else if (wasGroundedLastFrame && !isGrounded)
        {
            if (currentJump == 0)
            {
                currentJump = 1;
            }
        }

        animator.SetBool("isGrounded", isGrounded);
    }

    #endregion

    #region Helper Functions
    private Vector3 GetHorizontalRBVelocity()
    {
        return new Vector3(characterRigidbody.velocity.x, 0f, characterRigidbody.velocity.z);
    }

    private float GetMaxAllowedVelocity()
    {
        return moveSpeed * movementDirection.magnitude;
    }
    #endregion

    #region Misc Actions

    public void FlashlightToggle()
    {
        flashlight.enabled = !flashlight.enabled;
    }

    public void CameraSwitch()
    {
        if (playerCamera.Priority >= 10)
        {
            playerCamera.Priority = 0;
        } else if (playerCamera.Priority == 0) {
            playerCamera.Priority = 15;
        }
    }

    #endregion

    #region Debug

    private void OnDrawGizmos()
    {
        Vector3 overlapSphereOrigin = transform.position + (Vector3.up * (capsuleCollider.radius - groundCheckDistance));


        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(overlapSphereOrigin, capsuleCollider.radius);
    }

    #endregion
}