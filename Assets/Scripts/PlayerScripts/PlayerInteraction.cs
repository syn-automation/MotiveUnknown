using UnityEngine;
using Interactions;

namespace PlayerScripts
{
    public class PlayerInteraction : MonoBehaviour
    {
        public float interactRange = 2f;
        public LayerMask interactableLayer;
        public Transform playerCamera;
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                RaycastHit hit;
                if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, interactRange, interactableLayer))
                {
                    IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                    if (interactable != null)
                    {
                        interactable.Interact();
                    }
                }
            }
        }
    }
}

