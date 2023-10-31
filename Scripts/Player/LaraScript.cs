using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class LaraScript : MonoBehaviour
{

    [Header("Player Movement")]
    private Rigidbody2D rigidbody;
    private BoxCollider2D boxCollider2D;

    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float accelerationRate = 9f;
    [SerializeField] private float decelerationRate = 9f;
    [SerializeField] private float velPower = 0.9f;
    [SerializeField] private float frictionAmount = 0.8f;
    [SerializeField] private float jumpForce = 20f;
    private bool isGrounded = true;
    [SerializeField] private LayerMask environmentLayerMask;

    [Header("Player Input")]
    private Vector2 movementInput;

    [Header("Player Animation")]
    private SpriteRenderer spriteRenderer;
    private Animator animator;


    // Awake is called before Start() when an object is created or when the level is loaded
    private void Awake()
    {
        // Get component references
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.gravityScale = 5f;
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        CheckIsGrounded();

        MoveHorizontal();
    }

    // Update is called once per frame
    void Update()
    {

        if (movementInput.x > 0)
            spriteRenderer.flipX = false;
        // Otherwise if we are inputting left, face left
        else if (movementInput.x < 0)
            spriteRenderer.flipX = true;

        animator.SetBool("isRunning", (movementInput.x != 0f));
    }

    public void SetMovementInput(Vector2 moveInput)
    {
        movementInput = moveInput;
    }




    public void Jump()
    {
        if (isGrounded)
        {
            animator.SetTrigger("Jump");

            rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        }
    }


    public void CancelJump()
    {

        //if moving up
        if (rigidbody.velocity.y > 0)
        {
            // We can reduce our current y
            // velocity to cut our jump short
            rigidbody.AddForce(Vector2.down * rigidbody.velocity.y * 0.5f, ForceMode2D.Impulse);
        }
    }

    private void MoveHorizontal()
    {

        if (Mathf.Abs(movementInput.x) > 0.01f)
        {
            // Here we calculate the direction we want to move in
            // and our desired velocity in that direction.
            float targetSpeed = movementInput.x * moveSpeed;

            // Here we calculate the difference between our current
            // velocity and our previously calculated desired velocity.
            float speedDif = targetSpeed - rigidbody.velocity.x;

            //Determine if we are accelerating or decelerating.
            float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? accelerationRate : decelerationRate;

            float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);

            rigidbody.AddForce(movement * Vector2.right, ForceMode2D.Force);
        }

        else if (isGrounded)
        {
            // We use either the friction amount or our horizontal
            // velocity amount to counteract the velocity.
            float amount = Mathf.Min(Mathf.Abs(rigidbody.velocity.x), Mathf.Abs(frictionAmount));

            // Ensure that the direction we're applying friction in
            // is in the correct direction depending on our velocity.
            amount *= Mathf.Sign(rigidbody.velocity.x);

            //Apply the friction force in the direction opposite our velocity.
            rigidbody.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }

        animator.SetFloat("Speed", Mathf.Abs(rigidbody.velocity.x));

        animator.SetBool("IsFalling", (rigidbody.velocity.y < 0f));
    }


    private void CheckIsGrounded()
    {
        //Check for ground
        RaycastHit2D boxCastHitDown = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, 0.1f, environmentLayerMask);
        //Check for walls
        RaycastHit2D boxCastHitRight = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.right, 0.1f, environmentLayerMask);
        RaycastHit2D boxCastHitLeft = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.left, 0.1f, environmentLayerMask);

        isGrounded = (boxCastHitDown.collider != null) || (boxCastHitRight.collider != null) || (boxCastHitLeft.collider != null);

        animator.SetBool("IsGrounded", isGrounded);
        animator.SetBool("IsFalling", isGrounded);
    }
}
