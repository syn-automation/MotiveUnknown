using UnityEngine;

namespace ControllerScripts
{
    public class ThirdPersonAnimator : ThirdPersonMotor
    {
        #region Variables                

        public const float walkSpeed = 0.5f;
        public const float runningSpeed = 1f;
        public const float sprintSpeed = 1.5f;
        public const float crouchSpeed = 0.35f;
        public const float slideSpeed = 1.2f;

        #endregion  

        public virtual void UpdateAnimator()
        {
            if (animator == null || !animator.enabled) return;

            animator.SetBool(AnimatorParameters.IsStrafing, isStrafing); ;
            animator.SetBool(AnimatorParameters.IsSprinting, isSprinting);
            animator.SetBool(AnimatorParameters.IsGrounded, isGrounded);
            animator.SetBool(AnimatorParameters.IsCrouching, isCrouching);
            animator.SetFloat(AnimatorParameters.GroundDistance, groundDistance);

            if (isStrafing)
            {
                animator.SetFloat(AnimatorParameters.InputHorizontal, stopMove ? 0 : horizontalSpeed, strafeSpeed.animationSmooth, Time.deltaTime);
                animator.SetFloat(AnimatorParameters.InputVertical, stopMove ? 0 : verticalSpeed, strafeSpeed.animationSmooth, Time.deltaTime);
            }
            else
            {
                animator.SetFloat(AnimatorParameters.InputVertical, stopMove ? 0 : verticalSpeed, freeSpeed.animationSmooth, Time.deltaTime);
            }

            animator.SetFloat(AnimatorParameters.InputMagnitude, stopMove ? 0f : inputMagnitude, isStrafing ? strafeSpeed.animationSmooth : freeSpeed.animationSmooth, Time.deltaTime);
        }

        public virtual void SetAnimatorMoveSpeed(vMovementSpeed speed)
        {
            Vector3 relativeInput = transform.InverseTransformDirection(moveDirection);
            verticalSpeed = relativeInput.z;
            horizontalSpeed = relativeInput.x;

            var newInput = new Vector2(verticalSpeed, horizontalSpeed);
            if (isCrouching)
                inputMagnitude = Mathf.Clamp(newInput.magnitude, 0, crouchSpeed);
            else if (isSliding)
                inputMagnitude = Mathf.Clamp(newInput.magnitude, 0, slideSpeed);
            else if (speed.walkByDefault)
                inputMagnitude = Mathf.Clamp(newInput.magnitude, 0, isSprinting ? runningSpeed : walkSpeed);
            else
                inputMagnitude = Mathf.Clamp(isSprinting ? newInput.magnitude + 0.5f : newInput.magnitude, 0, isSprinting ? sprintSpeed : runningSpeed);
        }
    }

    public static partial class AnimatorParameters
    {
        public static int InputHorizontal = Animator.StringToHash("InputHorizontal");
        public static int InputVertical = Animator.StringToHash("InputVertical");
        public static int InputMagnitude = Animator.StringToHash("InputMagnitude");
        public static int IsGrounded = Animator.StringToHash("IsGrounded");
        public static int IsStrafing = Animator.StringToHash("IsStrafing");
        public static int IsSprinting = Animator.StringToHash("IsSprinting");
        public static int GroundDistance = Animator.StringToHash("GroundDistance");
        public static int IsCrouching = Animator.StringToHash("IsCrouching");
        public static int IsSliding = Animator.StringToHash("IsSliding");
    }
}