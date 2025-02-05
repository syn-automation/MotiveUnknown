using System;
using UnityEngine;

namespace PlayerScripts.Murderer
{
    public class FP_PlayerController : MonoBehaviour
    {
        [SerializeField] private float _animBlendSpeed = 8.9f;
        [SerializeField] private Transform CameraRoot;
        [SerializeField] private Transform Camera;
        [SerializeField] private float UpperLimit = -40f;
        [SerializeField] private float LowerLimit = 70f;
        [SerializeField] private float MouseSensitivity = 21.9f;
        [SerializeField] private float JumpFactor = 260f;
        [SerializeField] private float distanceToGround = 0.9f;
        [SerializeField] private LayerMask groundLayerMask;
        [SerializeField] private float airResistance = 0.2f;
        
            
        
        private Rigidbody _rigidbody;
        private FP_InputManager _inputManager;
        private Animator _animator;
        
        private bool _hasAnimator;
        private bool isGrounded;
        
        private int _xVelHash;
        private int _yVelHash;
        private int _zVelHash;
        private int _jumpHash;
        private int _groundHash;
        private int _fallingHash;
        
        private float _xRotation;
        

        private const float _walkSpeed = 2f;
        private const float _runSpeed = 6f;
        
        private Vector2 _currentVelocity;

        private void Start()
        {
            _hasAnimator = TryGetComponent<Animator>(out _animator);
            _rigidbody = GetComponent<Rigidbody>();
            _inputManager = GetComponent<FP_InputManager>();
            
            _xVelHash = Animator.StringToHash("X_Velocity");
            _yVelHash = Animator.StringToHash("Y_Velocity");
            _jumpHash = Animator.StringToHash("Jump");
            _groundHash = Animator.StringToHash("Grounded");
            _fallingHash = Animator.StringToHash("Falling");
            _zVelHash = Animator.StringToHash("Z_Velocity");
            
        }

        private void FixedUpdate()
        {
            Move();
            HandleJump();
            SampleGround();
        }

        private void LateUpdate()
        {
            CameraMovements();
        }

        private void Move()
        {
            if (!_hasAnimator)
            {
                return;
            }

            if (isGrounded)
            {
                float targetSpeed = _inputManager.Run ? _runSpeed : _walkSpeed;
            
                if(_inputManager.Move == Vector2.zero) targetSpeed = 0f;
            
                _currentVelocity.x = Mathf.Lerp(_currentVelocity.x, targetSpeed * _inputManager.Move.x, _animBlendSpeed * Time.deltaTime);
                _currentVelocity.y = Mathf.Lerp(_currentVelocity.y, targetSpeed * _inputManager.Move.y, _animBlendSpeed * Time.deltaTime);
            
                var xVelDifference = _currentVelocity.x - _rigidbody.linearVelocity.x;
                var yVelDifference = _currentVelocity.y - _rigidbody.linearVelocity.y;
            
                _rigidbody.AddForce(transform.TransformVector(new Vector3(_currentVelocity.x, 0, _currentVelocity.y)), ForceMode.VelocityChange);
            }
            else
            {
                _rigidbody.AddForce(transform.TransformVector(new Vector3(_currentVelocity.x * airResistance, 0, _currentVelocity.y * airResistance)), ForceMode.VelocityChange);
            }

            
            _animator.SetFloat(_xVelHash, _currentVelocity.x);
            _animator.SetFloat(_yVelHash, _currentVelocity.y);
            
        }

        private void CameraMovements()
        {
            if(!_hasAnimator) return;
        
            var MouseX = _inputManager.Look.x;
            var MouseY = _inputManager.Look.y;
            Camera.position = CameraRoot.position;
            
            _xRotation -= MouseY * MouseSensitivity * Time.smoothDeltaTime;
            _xRotation = Mathf.Clamp(_xRotation, UpperLimit, LowerLimit);
            
            Camera.localRotation = Quaternion.Euler(_xRotation, 0, 0);
            _rigidbody.MoveRotation(_rigidbody.rotation * Quaternion.Euler(0, MouseX * MouseSensitivity * Time.deltaTime, 0));
        }

        private void HandleJump()
        {
            if(!_hasAnimator) return;
            if (!_inputManager.Jump) return;
            
            _animator.SetTrigger(_jumpHash);
        }

        public void JumpAddForce()
        {
            _rigidbody.AddForce(-_rigidbody.linearVelocity.y * Vector3.up, ForceMode.VelocityChange);
            _rigidbody.AddForce(Vector3.up * JumpFactor, ForceMode.Impulse);
            _animator.ResetTrigger(_jumpHash);
        }

        private void SampleGround()
        {
            if (!_hasAnimator) return;

            if (Physics.Raycast(_rigidbody.worldCenterOfMass, Vector3.down, out _, distanceToGround + 0.1f,
                    groundLayerMask))
            {
                //Collision Detected - Grounded
                isGrounded = true;
                SetLandingAnimation();
                return;
            }
            
            isGrounded = false;
            _animator.SetFloat(_zVelHash, _rigidbody.linearVelocity.z);
            SetLandingAnimation();
            return;
        }

        private void SetLandingAnimation()
        {
            _animator.SetBool(_fallingHash, !isGrounded);
            _animator.SetBool(_groundHash, isGrounded);
        }
    }
}
