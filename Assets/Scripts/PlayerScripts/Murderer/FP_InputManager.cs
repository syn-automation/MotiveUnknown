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
        
        public bool Jump { get; private set; }
        
        public bool Interact { get; private set; }
        
        public bool RotateObjectLeft { get; private set; }
        
        public bool DropObject { get; private set; }
        
        public bool PickupObject { get; private set; }
        
        public bool OpenInventory { get; private set; }
        
        private InputActionMap _currentMap;
        
        private InputAction _moveAction;
        
        private InputAction _lookAction;
        
        private InputAction _runAction;
        
        private InputAction _jumpAction;
        
        private InputAction _interactAction;
        
        private InputAction _rotateObjectAction;
        
        private InputAction _dropObjectAction;
        
        private InputAction _pickupObjectAction;
        
        private InputAction _openInventoryAction;

        private void Awake()
        {
            _currentMap = playerInput.currentActionMap;
            _moveAction = _currentMap.FindAction("Move");
            _lookAction = _currentMap.FindAction("Look");
            _runAction = _currentMap.FindAction("Run");
            _jumpAction = _currentMap.FindAction("Jump");
            _interactAction = _currentMap.FindAction("Interact");
            _rotateObjectAction = _currentMap.FindAction("RotateObject");
            _dropObjectAction = _currentMap.FindAction("Drop");
            _pickupObjectAction = _currentMap.FindAction("Pickup");
            _openInventoryAction = _currentMap.FindAction("OpenInventory");
            
            _moveAction.performed += OnMove;
            _lookAction.performed += OnLook;
            _runAction.performed += OnRun;
            _jumpAction.performed += OnJump;
            _interactAction.performed += OnInteract;
            _rotateObjectAction.performed += OnRotateObject;
            _dropObjectAction.performed += OnDrop;
            _pickupObjectAction.performed += OnPickup;
            _openInventoryAction.performed += OnOpenInventory;
            
            _moveAction.canceled += OnMove;
            _lookAction.canceled += OnLook;
            _runAction.canceled += OnRun;
            _jumpAction.canceled += OnJump;
            _interactAction.canceled += OnInteract;
            _rotateObjectAction.canceled += OnRotateObject;
            _dropObjectAction.canceled += OnDrop;
            _pickupObjectAction.canceled += OnPickup;
            _openInventoryAction.canceled += OnOpenInventory;

        }

        private void OnMove(InputAction.CallbackContext context)
        {
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

        private void OnJump(InputAction.CallbackContext context)
        {
            Jump = context.ReadValueAsButton();
        }

        private void OnInteract(InputAction.CallbackContext context)
        {
            Interact = context.ReadValueAsButton();
        }

        private void OnRotateObject(InputAction.CallbackContext context)
        {
            RotateObjectLeft = context.ReadValueAsButton();
        }

        private void OnDrop(InputAction.CallbackContext context)
        {
            DropObject = context.ReadValueAsButton();
        }

        private void OnPickup(InputAction.CallbackContext context)
        {
            PickupObject = context.ReadValueAsButton();
        }

        private void OnOpenInventory(InputAction.CallbackContext context)
        {
            Debug.Log("Inventory Button Pressed");
            OpenInventory = context.ReadValueAsButton();
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