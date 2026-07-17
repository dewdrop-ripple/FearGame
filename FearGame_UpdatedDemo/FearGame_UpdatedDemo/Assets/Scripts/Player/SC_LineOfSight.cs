using UnityEngine;

public class SC_LineOfSight : MonoBehaviour
{
    private SC_GameManager gameManager;

    private SC_Item targetItem = null;
    private SC_StorageObject taregtStorageObject = null;

    private void Start()
    {
        gameManager = FindAnyObjectByType<SC_GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            targetItem = other.GetComponent<SC_Item>();
        }
        else if (other.tag == "Corpse")
        {
            taregtStorageObject = other.GetComponent<SC_StorageObject>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Item")
        {
            targetItem = other.GetComponent<SC_Item>();
        }
        else if (other.tag == "Corpse")
        {
            taregtStorageObject = other.GetComponent<SC_StorageObject>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Item")
        {
            targetItem = null;
        }
        else if (other.tag == "Corpse")
        {
            taregtStorageObject = null;
        }
    }

    public void UseTargetedItem()
    {
        if (targetItem)
        {
            targetItem.PickUp(gameManager.GetInventory());
        }
        else if (taregtStorageObject)
        {
            taregtStorageObject.Open();
        }
    }    
}
