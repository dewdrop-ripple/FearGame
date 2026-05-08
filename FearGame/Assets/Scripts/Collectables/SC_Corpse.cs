using System.Collections.Generic;
using UnityEngine;

public class SC_Corpse : MonoBehaviour
{
    // ----- VARIABLES ----- //

    [SerializeField] private List<SC_Collectable> mHeldItems;


    // ----- FUNCTIONS ----- //

    private void Start()
    {
        DisableAllItems();
    }

    public int GetNumItems()
    {
        return mHeldItems.Count;
    }

    public SC_Collectable GetItem(int index)
    {
        return mHeldItems[index];
    }

    public void RemoveItem(int index)
    {
        mHeldItems.RemoveAt(index);
    }

    public void DisableAllItems()
    {
        for (int i = 0; i < mHeldItems.Count; i++)
        {
            mHeldItems[i].gameObject.GetComponent<Renderer>().enabled = false;
            mHeldItems[i].gameObject.GetComponent<Collider>().enabled = false;
            mHeldItems[i].gameObject.GetComponent<Rigidbody>().detectCollisions = false;
        }
    }
}
