using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
    public class ItemData : ScriptableObject
    {
        public string itemName;
        public int itemID;
        public Sprite icon;
        public string description;
        public ItemType itemType;
        public bool isStackable;
        public int maxStackSize;
        public GameObject prefab;
    }
}