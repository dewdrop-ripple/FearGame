using UnityEngine;

public class SC_LineOfSight : MonoBehaviour
{
    private SC_GameManager gameManager;

    private SC_Item targetItem = null;

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
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Item")
        {
            targetItem = other.GetComponent<SC_Item>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Item")
        {
            targetItem = null;
        }
    }

    public void UseTargetedItem()
    {
        if (targetItem)
        {
            targetItem.PickUp(gameManager.GetInventory());
        }
    }    
}
