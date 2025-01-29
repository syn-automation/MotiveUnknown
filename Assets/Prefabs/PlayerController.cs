using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2 moveDirection;
    private float jumpDirection;
    public float moveSpeed = 2f;
    public float maxForwardSpeed = 8f;
    public float turnSpeed = 100f;
    public float crouchSpeed = 1.5f;
    public float sprintSpeed = 12f;
    private float desiredSpeed;
    private float forwardSpeed;
    private float jumpSpeed = 300f;
    private bool readyJump = false;
    private float jumpEffort = 1f; //Used with stats later on

    public float stamina = 100f;
    private float staminaMax = 100f;
    public float staminaRegen = 10f;
    public float staminaDrainRate = 20f;

    private float originalHeight;
    private float crouchHeight = 1f;
    
    const float groundAcceleration = 5f;
    const float groundDeceleration = 25f;
    
    private Animator animator;
    private new Rigidbody rigidbody;
    private new CapsuleCollider capsuleCollider;

    private bool onGround = true;
    private bool isCrouching = false;
    private bool isSprinting = false;
    
    bool IsMoveInput => !Mathf.Approximately(moveDirection.sqrMagnitude, 0f);

    public void OnMove(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
        
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        jumpDirection = context.ReadValue<float>();
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Crouch();
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isSprinting = true;
        }
        else if (context.canceled)
        {
            isSprinting = false;
        }
    }

    private void Move(Vector2 moveDirection)
    {
        float turnAmount = moveDirection.x;
        float fDirection = moveDirection.y;
        
        if (moveDirection.sqrMagnitude > 1f)
        {
            moveDirection.Normalize();
        }
        
        float currentMaxSpeed = isCrouching ? crouchSpeed : (isSprinting ? sprintSpeed : maxForwardSpeed);
        desiredSpeed = moveDirection.magnitude * maxForwardSpeed * Mathf.Sign(fDirection);
        
        float acceleration = IsMoveInput ? groundAcceleration : groundDeceleration;
        
        forwardSpeed = Mathf.MoveTowards(forwardSpeed, desiredSpeed, acceleration * Time.deltaTime);
        
        animator.SetFloat("CrouchSpeed", isCrouching ? Mathf.Abs(forwardSpeed) : 0f);
        animator.SetFloat("ForwardSpeed", forwardSpeed);
        animator.SetBool("IsSprinting", isSprinting);
        
        transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
    }


    private void Jump(float jumpDirection)
    {
        
        if (jumpDirection > 0 && onGround)
        {
            animator.SetBool("ReadyJump", true);
            readyJump = true;
        }
        else if (readyJump)
        {
            animator.SetBool("Launch", true);
            readyJump = false;
            animator.SetBool("ReadyJump", false);
            
        }
    }

    public void Launch()
    {
        rigidbody.AddForce(0, jumpSpeed * jumpEffort, 0);
        animator.SetBool("Launch", false);
        animator.applyRootMotion = false;
    }

    public void Land()
    {
        animator.SetBool("Land", false);
        animator.applyRootMotion = true;
        animator.SetBool("Launch", false);
    }

    private void Crouch()
    {
        isCrouching = !isCrouching;
        
        //Update collider Height
        capsuleCollider.height = isCrouching ? crouchHeight : originalHeight;
        capsuleCollider.center = new Vector3(0, capsuleCollider.height / 2, 0);
        
        animator.SetBool("Crouch", isCrouching);
    }

    private void StaminaUpdate()
    {
        if (isSprinting && stamina > 0)
        {
            stamina -= staminaDrainRate * Time.deltaTime;
            if (stamina <= 0)
            {
                stamina = 0;
                isSprinting = false;
                animator.SetBool("IsSprinting", false);
            }
        }
        else if (!isSprinting && stamina < staminaMax)
        {
            stamina += staminaRegen * Time.deltaTime;
            if (stamina >= staminaMax)
            {
                stamina = staminaMax;
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        originalHeight = capsuleCollider.height;
    }

    // Update is called once per frame
    private float groundRayDist = 1f;
    void Update()
    {
        Move(moveDirection);
        Jump(jumpDirection);

        StaminaUpdate();

        RaycastHit hit;
        Ray ray = new Ray(transform.position + Vector3.up * (groundRayDist * 0.5f), -Vector3.up);
        if (Physics.Raycast(ray, out hit, groundRayDist))
        {
            if (!onGround)
            {
                onGround = true;
                animator.SetBool("Land", true);
            }
        }
        else
        {
            onGround = false;
        }
        Debug.DrawRay(transform.position + Vector3.up * (groundRayDist * 0.5f), -Vector3.up * groundRayDist, Color.red);
    }
}
