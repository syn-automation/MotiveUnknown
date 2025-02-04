using System.Collections;
using UnityEngine;

namespace ControllerScripts
{
    public class ThirdPersonController : ThirdPersonAnimator
    {
        private static readonly int IsCrouching = Animator.StringToHash("IsCrouching");
        private static readonly int IsSliding = Animator.StringToHash("IsSliding");


        public virtual void ControlAnimatorRootMotion()
        {
            if (!this.enabled) return;

            if (inputSmooth == Vector3.zero)
            {
                transform.position = animator.rootPosition;
                transform.rotation = animator.rootRotation;
            }

            if (useRootMotion)
                MoveCharacter(moveDirection);
        }

        public virtual void ControlLocomotionType()
        {
            if (lockMovement) return;

            if (isCrouching)
            {
                SetControllerMoveSpeed(new vMovementSpeed() {walkSpeed = crouchSpeed});
                SetAnimatorMoveSpeed(new vMovementSpeed() {walkSpeed = crouchSpeed});
            }
            else if (locomotionType.Equals(LocomotionType.FreeWithStrafe) && !isStrafing || locomotionType.Equals(LocomotionType.OnlyFree))
            {
                SetControllerMoveSpeed(freeSpeed);
                SetAnimatorMoveSpeed(freeSpeed);
            }
            else if (locomotionType.Equals(LocomotionType.OnlyStrafe) || locomotionType.Equals(LocomotionType.FreeWithStrafe) && isStrafing)
            {
                isStrafing = true;
                SetControllerMoveSpeed(strafeSpeed);
                SetAnimatorMoveSpeed(strafeSpeed);
            }

            if (!useRootMotion)
                MoveCharacter(moveDirection);
        }

        public virtual void ControlRotationType()
        {
            if (lockRotation) return;

            bool validInput = input != Vector3.zero || (isStrafing ? strafeSpeed.rotateWithCamera : freeSpeed.rotateWithCamera);

            if (validInput)
            {
                // calculate input smooth
                inputSmooth = Vector3.Lerp(inputSmooth, input, (isStrafing ? strafeSpeed.movementSmooth : freeSpeed.movementSmooth) * Time.deltaTime);

                Vector3 dir = (isStrafing && (!isSprinting || sprintOnlyFree == false) || (freeSpeed.rotateWithCamera && input == Vector3.zero)) && rotateTarget ? rotateTarget.forward : moveDirection;
                RotateToDirection(dir);
            }
        }

        public virtual void UpdateMoveDirection(Transform referenceTransform = null)
        {
            if (input.magnitude <= 0.01)
            {
                moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, (isStrafing ? strafeSpeed.movementSmooth : freeSpeed.movementSmooth) * Time.deltaTime);
                return;
            }

            if (referenceTransform && !rotateByWorld)
            {
                //get the right-facing direction of the referenceTransform
                var right = referenceTransform.right;
                right.y = 0;
                //get the forward direction relative to referenceTransform Right
                var forward = Quaternion.AngleAxis(-90, Vector3.up) * right;
                // determine the direction the player will face based on input and the referenceTransform's right and forward directions
                moveDirection = (inputSmooth.x * right) + (inputSmooth.z * forward);
            }
            else
            {
                moveDirection = new Vector3(inputSmooth.x, 0, inputSmooth.z);
            }
        }

        public virtual void Sprint(bool value)
        {
            if (isCrouching) return;
            
            var sprintConditions = (input.sqrMagnitude > 0.1f && stamina > 0 && isGrounded &&
                !(isStrafing && !strafeSpeed.walkByDefault && (horizontalSpeed >= 0.5 || horizontalSpeed <= -0.5 || verticalSpeed <= 0.1f)));

            if (value && sprintConditions)
            {
                if (input.sqrMagnitude > 0.1f)
                {
                    if (isGrounded && useContinuousSprint)
                    {
                        isSprinting = !isSprinting;
                    }
                    else if (!isSprinting)
                    {
                        isSprinting = true;
                    }
                }
                else if (!useContinuousSprint && isSprinting)
                {
                    isSprinting = false;
                }
            }
            else if (isSprinting)
            {
                isSprinting = false;
            }
        }

        public virtual void Strafe()
        {
            isStrafing = !isStrafing;
        }

        public virtual void Jump()
        {
            if (isCrouching) return;
            
            // trigger jump behaviour
            jumpCounter = jumpTimer;
            isJumping = true;

            // trigger jump animations
            if (input.sqrMagnitude < 0.1f)
                animator.CrossFadeInFixedTime("Jump", 0.1f);
            else
                animator.CrossFadeInFixedTime("JumpMove", .2f);
        }

        public virtual void Crouch()
        {
            if (isSprinting)
            {
                StartSlide();
                return;
            }
            if (!isCrouching)
            {
                isCrouching = true;
                animator.SetBool(IsCrouching, isCrouching);
            }
            else
            {
                if (!Physics.Raycast(transform.position, Vector3.up, standingHeight - crouchHeight + 0.1f,
                        crouchObstructionLayers))
                {
                    isCrouching = false;
                    animator.SetBool(IsCrouching, isCrouching);
                }
            }
        }

        public virtual void StartSlide()
        {
            if (isSliding || !canSlide) return;
            
            isSliding = true;
            canSlide = false;
            animator.SetBool(IsSliding, isSliding);
            moveSpeed *= slideSpeed;
            
            StartCoroutine(EndSlide(sprintSpeed));
            StartCoroutine(SlideCoolDown());
        }

        private IEnumerator EndSlide(float speed)
        {
            yield return new WaitForSeconds(slideTime);
            
            isSliding = false;
            moveSpeed = speed;
            animator.SetBool(IsSliding, isSliding);
        }

        private IEnumerator SlideCoolDown()
        {
            yield return new WaitForSeconds(slideCoolDown);
            canSlide = true;
        }


    }
}