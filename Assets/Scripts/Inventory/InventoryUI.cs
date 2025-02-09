using System;
using PlayerScripts.Murderer;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using System.Collections.Generic;

namespace Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        public GameObject inventoryPanel;
        public TextMeshProUGUI itemListText;
        private FP_InputManager _inputManager;
        private bool isOpen = false;

        private void Awake()
        {
            _inputManager = FindFirstObjectByType<FP_InputManager>();
        
            if (_inputManager != null)
            {
                Debug.Log("Input Manager is not null");
            }
        }


        private void Start()
        {
            Debug.Log("Starting Inventory ");
            inventoryPanel.GetComponent<CanvasGroup>().alpha = 0;
            inventoryPanel.GetComponent<CanvasGroup>().interactable = false;
            inventoryPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        private void Update()
        {
            if (_inputManager.OpenInventory)
            {
                ToggleInventory();
            }
        }

        
        public void ToggleInventory()
        {
            CanvasGroup canvasGroup = inventoryPanel.GetComponent<CanvasGroup>();
            
            if (canvasGroup.alpha == 1 && !isOpen)
            {
                canvasGroup.alpha = 0;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
                isOpen = true;
            }
            else
            {
                UpdateInventoryDisplay();
                canvasGroup.alpha = 1;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
                isOpen = false;
            }
        }
        
        private void UpdateInventoryDisplay()
        {

        }
    }

}