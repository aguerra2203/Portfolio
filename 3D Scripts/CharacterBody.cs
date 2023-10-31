using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public abstract class CharacterBody : MonoBehaviour
{
    [Header("Character - Ground Movement")]
    public float moveSpeed = 10f;

    [Header("Character - Air Movement")]
    [SerializeField] protected float jumpForce = 10f;

    [Header("Character - Rotation")]
    public float rotationSpeed = 10f;

    [Header("Character - Input")]
    protected Vector3 movementDirection;

    [Header("Character - Component/Object References")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected CapsuleCollider capsuleCollider;
    [SerializeField] protected Rigidbody characterRigidbody;
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected Transform characterModel;

    #region Input

    public virtual void SetMoveInput(Vector2 moveInput)
    {
        throw new System.NotImplementedException();
    }

    #endregion

    #region Movement

    //Called in every FixedUpdate
    protected virtual void MoveCharacter()
    {
        throw new System.NotImplementedException();
    }

    //Called in every Update
    protected virtual void RotateCharacter()
    {
        throw new System.NotImplementedException();
    }

    public virtual void Jump()
    {
        throw new System.NotImplementedException();
    }

    public virtual void JumpCanceled()
    {
        throw new System.NotImplementedException();
    }

    #endregion

}
