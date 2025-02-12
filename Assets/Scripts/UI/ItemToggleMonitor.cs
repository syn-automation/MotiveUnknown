using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Inventory;
using UnityEngine.XR;

namespace UI
{
    public class ItemToggleMonitor : MonoBehaviour
    {
        
        public ToggleGroup toggleGroup;
        
        private Toggle lastSelectedToggle;
        
        void Start()
        {
            foreach (var toggle in toggleGroup.GetComponentsInChildren<Toggle>())
            {
                toggle.onValueChanged.AddListener(isOn =>
                {
                    if (isOn)
                    {
                        HandleToggleChange(toggle);
                    }
                });
            }
        }

        private void HandleToggleChange(Toggle toggle)
        {
            if (toggle != lastSelectedToggle)
            {
                lastSelectedToggle = toggle;
                ItemType toggleType = toggle.GetComponent<ToggleItemType>().itemType;
            }
        }
    }
}
