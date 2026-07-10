using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

struct InventoryItem
{
    public SC_Item item;
    public int slot;
}

public class SC_ItemMenuManager : MonoBehaviour
{
    // --- UI LAYOUT INFO --- //

    [SerializeField, UnityEngine.Range(0.0f, 1.0f)] private float slotSpacing; // In Percent of Panel

    [SerializeField] private List<GameObject> slots;
    [SerializeField] private int columns;
    [SerializeField] private int rows;

    [SerializeField] private GameObject infoPanel;
    [SerializeField] private GameObject backgroundPanel;


    // --- ITEM INFO --- //

    private List<InventoryItem> containedItems;


    // --- GENERAL FUNCTIONS --- //

    private void Update()
    {
        UpdateSlotLayout();
    }


    // --- UI FUNCTIONS --- //

    private void UpdateSlotLayout()
    {
        // Panel Size

        float width = backgroundPanel.GetComponent<RectTransform>().rect.width;
        float height = backgroundPanel.GetComponent<RectTransform>().rect.height;


        // Slot Sizes

        float pixelSlotSpacingW = slotSpacing * width;
        float pixelSlotSizeW = (width - (pixelSlotSpacingW * (columns + 1))) / columns;

        float pixelSlotSpacingH = slotSpacing * height;
        float pixelSlotSizeH = (height - (pixelSlotSpacingH * (rows + 1))) / rows;


        // Resize everything

        for (int targetCol = 0; targetCol < columns; targetCol++)
        {
            for (int taregtRow = 0; taregtRow < rows; taregtRow++)
            {
                int targetSlot = (targetCol * rows) + taregtRow;

                float targetX = (pixelSlotSpacingW * (targetCol + 1)) + (pixelSlotSizeW * targetCol);
                float targetY = (pixelSlotSpacingH * (taregtRow + 1)) + (pixelSlotSizeH * taregtRow);

                Debug.Log(targetX + ", " + targetY);

                slots[targetSlot].GetComponent<RectTransform>().rect.Set(targetX, targetY, pixelSlotSizeW, pixelSlotSizeH);
            }
        }
    }
}
