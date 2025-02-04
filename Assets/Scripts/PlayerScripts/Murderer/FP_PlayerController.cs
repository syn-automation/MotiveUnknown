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
        private Rigidbody _rigidbody;
        private FP_InputManager _inputManager;
        private Animator _animator;
        private bool _hasAnimator;
        private int _xVelHash;
        private int _yVelHash;
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
            
        }

        private void FixedUpdate()
        {
            Move();
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
            
            float targetSpeed = _inputManager.Run ? _runSpeed : _walkSpeed;
            
            if(_inputManager.Move == Vector2.zero) targetSpeed = 0.1f;
            
            _currentVelocity.x = Mathf.Lerp(_currentVelocity.x, targetSpeed * _inputManager.Move.x, _animBlendSpeed * Time.deltaTime);
            _currentVelocity.y = Mathf.Lerp(_currentVelocity.y, targetSpeed * _inputManager.Move.y, _animBlendSpeed * Time.deltaTime);
            
            var xVelDifference = _currentVelocity.x - _rigidbody.linearVelocity.x;
            var yVelDifference = _currentVelocity.y - _rigidbody.linearVelocity.y;
            
            _rigidbody.AddForce(transform.TransformVector(new Vector3(_currentVelocity.x, 0, _currentVelocity.y)), ForceMode.VelocityChange);
            
            _animator.SetFloat(_xVelHash, _currentVelocity.x);
            _animator.SetFloat(_yVelHash, _currentVelocity.y);
            
        }

        private void CameraMovements()
        {
            if(_hasAnimator) return;

            var MouseX = _inputManager.Look.x;
            var MouseY = _inputManager.Look.y;
            Camera.position = CameraRoot.position;
            
            _xRotation -= MouseY * MouseSensitivity * Time.deltaTime;
            _xRotation = Mathf.Clamp(_xRotation, UpperLimit, LowerLimit);
            
            Camera.localRotation = Quaternion.Euler(_xRotation, 0, 0);
            transform.Rotate(Vector3.up, MouseX * MouseSensitivity * Time.deltaTime);
        }
    }
}
