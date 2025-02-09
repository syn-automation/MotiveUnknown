using Interactions;
using UnityEngine;

namespace Objects
{
    public class Bottle : MonoBehaviour, IInteractable
    {
        public string itemName = "Bottle";

        public void Interact()
        {
            Debug.Log("Picked up item" + itemName);
            Destroy(gameObject);
        }
        
    }
}
