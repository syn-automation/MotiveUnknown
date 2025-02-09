using UnityEngine;

namespace Inventory
{   
    [System.Serializable]
    public class InventorySlot
    {
        public ItemData itemData;
        public int quantity;

        public InventorySlot(ItemData itemData, int quantity)
        {
            this.itemData = itemData;
            this.quantity = quantity;
        }

        public bool isEmpty()
        {
            return itemData == null || quantity <= 0;
        }

        public void ClearSlot()
        {
            itemData = null;
            quantity = 0;
        }

        public void SetItem(ItemData newItemData, int newQuantity)
        {
            itemData = newItemData;
            quantity = newQuantity;
        }
    }

}