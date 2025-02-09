using System;
using System.Collections;
using UnityEngine;
using Interactions;
using PlayerScripts.Murderer;
using TMPro;

namespace PlayerScripts
{
    public class PlayerInteraction : MonoBehaviour
    {
        private FP_InputManager _inputManager;
        public float interactRange = 2f;
        public LayerMask interactableLayer;
        public Transform playerCamera;
        public Transform inspectionPoint;
        public TextMeshProUGUI interactionText;

        private IInteractable currentInteractable;
        private InteractableObject highlightedObject = null;
        private InteractableObject inspectedObject = null;


        private void Awake()
        {
            _inputManager = GetComponent<FP_InputManager>();
        }

        void Update()
        {
            if (inspectedObject == null)
            {
                Interaction();
            }
            else
            {
                HandleInspection();
            }
            
        }
        void Interaction()
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, interactRange, interactableLayer))
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                InteractableObject interactableObject = hit.collider.GetComponent<InteractableObject>();

                if (interactable != null)
                {
                    // Show text
                    interactionText.gameObject.SetActive(true);
                    interactionText.text = "Press E to Interact";

                    if (highlightedObject != interactableObject)
                    {
                        RemoveHighlight();
                        highlightedObject = interactableObject;
                        highlightedObject?.EnableHighlight(true);
                    }

                    currentInteractable = interactable;

                    if (_inputManager.Interact)
                    {
                        interactable.Interact();
                    }
                }
            }
            else
            {
                // Hide text if nothing is interactable
                interactionText.gameObject.SetActive(false);
                currentInteractable = null;
                RemoveHighlight();
            }
        }
        
        public void InspectObject(InteractableObject obj)
        {
            inspectedObject = obj;
            inspectedObject.SetInspectionState(true, FindAnyObjectByType<FP_PlayerController>().GetCameraTransform());
            
            FindFirstObjectByType<FP_PlayerController>().StartInspection();
            
            interactionText.text = "Left Click to Rotate | Right Click to Drop";
        }
        
        private void RemoveHighlight()
        {
            if (highlightedObject != null)
            {
                highlightedObject.EnableHighlight(false);
                highlightedObject = null;
            }
        }

        private void HandleInspection()
        {
            if (_inputManager.RotateObjectLeft)
            {
                Vector2 rotationInput = new Vector2(_inputManager.Look.x, _inputManager.Look.y);
                inspectedObject.RotateObject(rotationInput);
            }

            if (_inputManager.DropObject)
            {
                inspectedObject.SetInspectionState(false, null);
                inspectedObject = null;
                
                FindFirstObjectByType<FP_PlayerController>().StopInspection();
                
                interactionText.text = "";
            }

            if (_inputManager.PickupObject)
            {
                inspectedObject.Pickup();
                inspectedObject = null;
                
                FindFirstObjectByType<FP_PlayerController>().StopInspection();
                
                interactionText.text = "";
            }
        }
            

    }


}

