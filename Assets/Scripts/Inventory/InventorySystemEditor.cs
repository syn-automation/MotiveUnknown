using UnityEngine;
using UnityEditor;

namespace Inventory
{
    [CustomEditor(typeof(InventorySystem))]
    public class InventorySystemEditor : Editor
    {
        private Color lineColor = new Color(0.3f, 0.3f, 0.3f, 1.0f);
        private float rowHeight = 20.0f;

        public override void OnInspectorGUI()
        {
            InventorySystem inventorySystem = (InventorySystem)target;
            
            serializedObject.Update();
            
            EditorGUILayout.PropertyField(serializedObject.FindProperty("maxInventorySlots"));

            if (inventorySystem.maxInventorySlots == null || inventorySystem.maxInventorySlots == 0)
            {
                EditorGUILayout.LabelField("Inventory is empty.");
            }
            else
            {
                GUILayout.BeginVertical();
                DrawRowHeader();
            
                DrawHorizontalLine();

                for (int i = 0; i < inventorySystem.inventorySlots.Count; i++)
                {
                    DrawRow(inventorySystem.inventorySlots[i], i);
                    DrawHorizontalLine();
                }
            
                GUILayout.EndVertical();
            }
            
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawRowHeader()
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Item Name", EditorStyles.boldLabel, GUILayout.Width(200));
            EditorGUILayout.LabelField("Quantity", EditorStyles.boldLabel, GUILayout.Width(200));
            GUILayout.EndHorizontal();
        }

        private void DrawRow(InventorySlot slot, int index)
        {
            GUILayout.BeginHorizontal();
            string itemName = slot.itemData != null ? slot.itemData.itemName : "Empty Slot";
            EditorGUILayout.LabelField(itemName, GUILayout.Width(200));
            
            int quantity = slot.itemData != null ? slot.quantity : 0;
            EditorGUILayout.LabelField(quantity.ToString(), GUILayout.Width(200));
        }

        private void DrawHorizontalLine()
        {
            Rect rect = EditorGUILayout.GetControlRect(false, 1);
            rect.height = 1;
            EditorGUI.DrawRect(rect, lineColor);
        }
        
    }

}