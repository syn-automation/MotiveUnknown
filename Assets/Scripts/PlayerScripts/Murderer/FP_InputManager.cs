using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace PlayerScripts.Murderer
{
    public class FP_InputManager : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInput;
        
        public Vector2 Move { get; private set; }
        
        public Vector2 Look { get; private set; }
        
        public bool Run { get; private set; }
        
        private InputActionMap _currentMap;
        
        private InputAction _moveAction;
        
        private InputAction _lookAction;
        
        private InputAction _runAction;

        private void Awake()
        {
            _currentMap = playerInput.currentActionMap;
            _moveAction = _currentMap.FindAction("Move");
            _lookAction = _currentMap.FindAction("Look");
            _runAction = _currentMap.FindAction("Run");
            
            _moveAction.performed += OnMove;
            _lookAction.performed += OnLook;
            _runAction.performed += OnRun;
            
            _moveAction.canceled += OnMove;
            _lookAction.canceled += OnLook;
            _runAction.canceled += OnRun;
            
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            Debug.Log("Move called");
            Move = context.ReadValue<Vector2>();
        }

        private void OnLook(InputAction.CallbackContext context)
        {
            Look = context.ReadValue<Vector2>();
        }

        private void OnRun(InputAction.CallbackContext context)
        {
            Run = context.ReadValueAsButton();
        }

        private void OnEnable()
        {
            _currentMap.Enable();
        }

        private void OnDisable()
        {
            _currentMap.Disable();
        }
        
    }

}