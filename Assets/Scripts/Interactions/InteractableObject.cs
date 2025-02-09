using Inventory;
using PlayerScripts;
using UnityEngine;


namespace Interactions
{
    public class InteractableObject : MonoBehaviour, IInteractable
    {
        [SerializeField] private ItemData itemData;
        public int quantity = 1;
        private Outline outline;
        private bool isBeingInspected = false;
        private Transform originalParent;
        private Vector3 originalPosition;
        private Quaternion originalRotation;
        private Vector3 originalScale;
        private Rigidbody rb;
        private PlayerInventorySystem playerInventorySystem;
        
        
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            outline = GetComponent<Outline>();
            playerInventorySystem = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventorySystem>();
            if (outline != null) outline.enabled = false;

            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }
        }

        public void Interact()
        {
            Debug.Log("Interacted with " + gameObject.name);

            if (!isBeingInspected)
            {
                FindFirstObjectByType<PlayerInteraction>().InspectObject(this);
            }
        }

        public void EnableHighlight(bool enable)
        {
            if (outline != null) outline.enabled = enable;
        }

        public void SetInspectionState(bool inspecting, Transform cameraTransform)
        {
            isBeingInspected = inspecting;
            if (inspecting)
            {
                originalParent = transform.parent;
                originalPosition = transform.position;
                originalRotation = transform.rotation;
                originalScale = transform.localScale;
                
                
                transform.SetParent(cameraTransform);
                transform.position = cameraTransform.position + cameraTransform.forward * 0.5f;
                transform.localRotation = Quaternion.identity;
                
                transform.localScale = originalScale * 0.5f;
                
                if (rb) rb.isKinematic = true; //Disables physics
            }
            else
            {
                transform.SetParent(originalParent);
                transform.position = originalPosition;
                transform.rotation = originalRotation;
                transform.localScale = originalScale;
                
                
            }
        }
        
        public void RotateObject(Vector2 rotationInput)
        {
            if (isBeingInspected)
            {
                float rotationSpeed = 5f;
                transform.Rotate(Vector3.up, -rotationInput.x * rotationSpeed, Space.World);
                transform.Rotate(Vector3.right, rotationInput.y * rotationSpeed, Space.World);
            }
        }

        public void Pickup()
        {
            if (playerInventorySystem != null)
            {
                quantity = playerInventorySystem.PickUpItem(itemData, quantity);
                if (quantity <= 0)
                    Destroy(gameObject);
            }
        }

        public void Drop()
        {
            if (rb)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }
        }
    }
}
