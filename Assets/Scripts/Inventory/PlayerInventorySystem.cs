using System;
using PlayerScripts.Murderer;
using UnityEngine;

namespace Inventory
{
    public class PlayerInventorySystem : MonoBehaviour
    {
        public InventorySystem inventorySystem;
        private FP_InputManager _inputManager;

        private void Awake()
        {
            _inputManager = GetComponent<FP_InputManager>();
        }

        private void Update()
        {

        }

        public int PickUpItem(ItemData itemData, int quantity)
        {
            if (!inventorySystem.IsFull() || itemData.isStackable)
            {
                return inventorySystem.AddItem(itemData, quantity);
            }
            
            return quantity;
        }

        public void DropItemsFromSlot(int slotNumber)
        {
            inventorySystem.RemoveItemsFromSlot(slotNumber);
        }

        public void DropItem(ItemData itemData, int quantity)
        {
            int numberDropped = quantity - inventorySystem.RemoveItem(itemData, quantity);

            if (numberDropped > 0)
            {
                Instantiate(itemData.prefab, GetDropPosition(), Quaternion.Euler(0,0,0));
            }
            
        }

        private Vector3 GetDropPosition()
        {
            Vector3 playerPosition = transform.position;
            Vector3 forwardDirection = transform.forward;
            Vector3 upDirection = transform.up;
            
            float dropDistance = 2f;
            
            return playerPosition + forwardDirection * dropDistance + upDirection * dropDistance;
        }
        
    }
}
