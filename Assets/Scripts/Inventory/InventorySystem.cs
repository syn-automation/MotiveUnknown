using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Inventory
{
    public class InventorySystem : MonoBehaviour
    {
        public List<InventorySlot> inventorySlots = new List<InventorySlot>();
        public int maxInventorySlots = 10;

        public int AddItem(ItemData itemData, int quantity)
        {
            if (quantity <= 0) return 0;
            
            int remainingItems = quantity;

            if (itemData.isStackable)
            {
                foreach (InventorySlot slot in inventorySlots)
                {
                    if (slot.itemData == itemData)
                    {
                        int spaceInSlot = itemData.maxStackSize - slot.quantity;

                        if (spaceInSlot > 0)
                        {
                            int itemsToAdd = Mathf.Min(remainingItems, spaceInSlot);
                            slot.quantity += itemsToAdd;
                            remainingItems -= itemsToAdd;

                            if (remainingItems <= 0)
                            {
                                return 0;
                            }
                        }
                    }
                }
            }

            while (remainingItems > 0 && inventorySlots.Count < maxInventorySlots)
            {
                int itemsToAdd = Mathf.Min(remainingItems, itemData.isStackable ? itemData.maxStackSize : 0);
                
                inventorySlots.Add(new InventorySlot(itemData, quantity));
                
                remainingItems -= itemsToAdd;
                
            }
            
            return remainingItems;

        }

        public int RemoveItem(ItemData itemData, int quantity, bool removePartial = true)
        {
            int remainingItems = quantity;

            List<InventorySlot> slotsWithItem = inventorySlots.Where(s => s.itemData == itemData).ToList();
            
            int totalAvailableItems = slotsWithItem.Sum(s => s.quantity);
            if (remainingItems > totalAvailableItems && !removePartial)
            {
                return quantity;
            }

            foreach (var slot in slotsWithItem)
            {
                if (remainingItems <= 0)
                {
                    break;
                }

                if (slot.quantity < remainingItems)
                {
                    remainingItems -= slot.quantity;
                    slot.ClearSlot();
                    inventorySlots.Remove(slot);
                }
                else
                {
                    slot.quantity -= remainingItems;
                    remainingItems = 0;
                }
            }
            return remainingItems;
        }

        public void RemoveItemsFromSlot(int slotNumber)
        {
            inventorySlots.RemoveAt(slotNumber);
        }

        public bool IsFull()
        {
            return inventorySlots.Count >= maxInventorySlots;
        }
        
        public void ClearInventory()
        {
            inventorySlots.Clear();
        }
    }
}
