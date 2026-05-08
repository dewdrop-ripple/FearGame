using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class SC_CharacterInventory : MonoBehaviour
{
    // ----- VARIABLES ----- //

    [SerializeField] private List<SC_Collectable> mItems;

    [SerializeField] private SC_PlayerData mAttachedPlayer;

    [SerializeField] private int mMaxItems;


    // ----- FUNCTIONS ----- //

    public bool AddItem(SC_Collectable item)
    {
        if (mItems.Count < mMaxItems)
        {
            mItems.Add(item);
            //mItems.Sort();
            return true;
        }
        else
        {
            Debug.Log("Inventory Full");
            return false;
        }
    }

    public void RemoveItem(SC_Collectable item)
    {
        Destroy(item.gameObject);
        mItems.Remove(item);
    }

    public void RemoveItem(int index)
    {
        Destroy(mItems[index].gameObject);
        mItems.RemoveAt(index);
    }

    public void RemoveItemWithoutDestroying(SC_Collectable item)
    {
        mItems.Remove(item);
    }

    public void RemoveItemWithoutDestroying(int index)
    {
        mItems.RemoveAt(index);
    }

    public SC_Collectable GetItem(int index)
    {
        return mItems[index];
    }

    public void UseItem(SC_Collectable item)
    {
        item.UseItem(mAttachedPlayer);
        RemoveItem(item);
    }

    public void UseItem(int index)
    {
        GetItem(index).UseItem(mAttachedPlayer);
        RemoveItem(index);
    }

    public int GetNumItems()
    {
        return mItems.Count;
    }

    public bool Contains(SC_Collectable.CollectableType itemType)
    {
        for (int i = 0; i < mItems.Count; i++)
        {
            if (mItems[i].GetCollectableType() == itemType)
            {
                return true;
            }
        }

        return false;
    }

    /*
    public void Sort()
    {
        mItems.Sort();
    }
    */
}
