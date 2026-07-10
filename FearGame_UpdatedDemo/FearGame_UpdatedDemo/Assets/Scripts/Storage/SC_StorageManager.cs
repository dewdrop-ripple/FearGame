using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.Rendering.VolumeComponent;

public class SC_StorageManager : MonoBehaviour
{
    public enum StorageType
    {
        INVENTORY,
        OTHER
    }


    [SerializeField] private List<GameObject> itemSlotObjects;
    [SerializeField] private List<SC_ItemManager> items;

    [SerializeField] private GameObject inactiveItemsParent;
    [SerializeField] private GameObject activeItemsParent;

    [SerializeField] private StorageType storageType;

    private SC_GameManager gameManager;

    [SerializeField] private bool isOpen;


    private void Awake()
    {
        for (int i = 0; i < itemSlotObjects.Count; i++)
        {
            items.Add(null);
        }
    }

    private void Start()
    {
        gameManager = FindAnyObjectByType<SC_GameManager>();
    }

    private void Update()
    {
        if (storageType == StorageType.INVENTORY)
        {
            gameManager.SetInventory(this);
        }
        else if (storageType == StorageType.OTHER && isOpen)
        {
            gameManager.SetOpenStorageUnit(this);
        }
    }

    public int GetNearestSlot(Vector3 location)
    {
        int closestSlot = 0;
        float distance = Vector3.Distance(location, itemSlotObjects[0].transform.position);

        for (int i = 1; i < itemSlotObjects.Count; i++)
        {
            float checkDistance = Vector3.Distance(location, itemSlotObjects[i].transform.position);

            if (checkDistance < distance)
            {
                closestSlot = i;
                distance = checkDistance;
            }
        }

        float xDistance = Mathf.Abs(itemSlotObjects[closestSlot].transform.position.x - location.x);
        float yDistance = Mathf.Abs(itemSlotObjects[closestSlot].transform.position.y - location.y);

        if (xDistance < itemSlotObjects[closestSlot].GetComponent<RectTransform>().rect.width * Screen.width / 2500.0f * 1.45f)
        {
            if (yDistance < itemSlotObjects[closestSlot].GetComponent<RectTransform>().rect.height * Screen.height / 1250.0f * 1.35f)
            {
                return closestSlot;
            }
        }

        return -1;
    }

    public bool SlotIsOpen(int index)
    {
        if (index < 0 || index >= itemSlotObjects.Count)
        {
            return false;
        }

        return items[index] == null;
    }

    public Vector3 GetSlotPos(int index)
    {
        if (index < 0 || index >= itemSlotObjects.Count)
        {
            return new Vector3(0.0f, 0.0f, 0.0f);
        }

        return itemSlotObjects[index].transform.position;
    }

    public void RemoveItemFromSlot(int index)
    {
        if (index < 0 || index >= itemSlotObjects.Count)
        {
            return;
        }

        items[index] = null;
    }

    public void AddItemToSlot(int index, SC_ItemManager item)
    {
        if (index < 0 || index >= itemSlotObjects.Count)
        {
            return;
        }

        items[index] = item;
    }

    public GameObject GetInactiveParent()
    {
        return inactiveItemsParent;
    }

    public GameObject GetActiveParent()
    {
        return activeItemsParent;
    }

    public void SwapSlots(int index1, int index2)
    {
        SC_ItemManager temp = items[index1];
        items[index1] = items[index2];
        items[index2] = temp;

        items[index1].SetPosition(GetSlotPos(index1));
        items[index2].SetPosition(GetSlotPos(index2));

        items[index1].SetSlot(index1);
        items[index2].SetSlot(index2);
    }

    public void SwapAcrossUnits(SC_StorageManager otherUnit, int thisUnitSlot, int otherUnitSlot)
    {
        otherUnit.GetSlotItem(otherUnitSlot).SetStorageManager(this);
        GetSlotItem(thisUnitSlot).SetStorageManager(otherUnit);

        SC_ItemManager temp = items[thisUnitSlot];

        SetSlotItem(thisUnitSlot, otherUnit.GetSlotItem(otherUnitSlot));
        otherUnit.SetSlotItem(otherUnitSlot, temp);

        otherUnit.SetSlotPos(otherUnitSlot, otherUnit.GetSlotPos(otherUnitSlot));
        SetSlotPos(thisUnitSlot, GetSlotPos(thisUnitSlot));
        
        otherUnit.GetSlotItem(otherUnitSlot).SetSlot(otherUnitSlot);
        GetSlotItem(thisUnitSlot).SetSlot(thisUnitSlot);

        otherUnit.GetSlotItem(otherUnitSlot).transform.SetParent(otherUnit.GetInactiveParent().transform, true);
        GetSlotItem(thisUnitSlot).transform.SetParent(GetInactiveParent().transform, true);
    }

    public SC_ItemManager GetSlotItem(int index)
    {
        return items[index];
    }

    public void SetSlotItem(int index, SC_ItemManager item)
    {
        items[index] = item;
    }

    public void SetSlotPos(int index, Vector3 pos)
    {
        items[index].SetPosition(pos);
    }

    public StorageType GetStorageType()
    {
        return storageType;
    }
}
