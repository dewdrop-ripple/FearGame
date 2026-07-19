using UnityEngine;

public class SC_LineOfSight : MonoBehaviour
{
    private SC_GameManager gameManager;

    private SC_Item targetItem = null;
    private SC_StorageObject taregtStorageObject = null;
    private SC_NPC targetNPC = null;

    [SerializeField] private UnityEngine.UI.Image crosshair;
    [SerializeField] private Color noTargetColor;
    [SerializeField] private Color targetColor;

    private void Start()
    {
        gameManager = FindAnyObjectByType<SC_GameManager>();
    }

    private void Update()
    {
        if (targetItem == null && taregtStorageObject == null && targetNPC == null)
        {
            crosshair.color = noTargetColor;
        }
        else
        {
            crosshair.color = targetColor;
        }
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
        else if (other.tag == "NPC")
        {
            targetNPC = other.GetComponent<SC_NPC>();
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
        else if (other.tag == "NPC")
        {
            targetNPC = other.GetComponent<SC_NPC>();
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
        else if (other.tag == "NPC")
        {
            targetNPC = null;
        }
    }

    public void UseTargetedItem()
    {
        if (targetItem)
        {
            targetItem.PickUp(gameManager.GetInventory());
            targetItem = null;
        }
        else if (taregtStorageObject)
        {
            taregtStorageObject.Open();
        }
        else if (targetNPC)
        {
            targetNPC.Open();
        }
    }    
}
