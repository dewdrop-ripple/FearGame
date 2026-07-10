using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class SC_ItemSlotOld : MonoBehaviour
{
    [SerializeField] private SC_ItemOld mAttachedItem; // Default: null

    [SerializeField] private bool mLock = true;

    private void Update()
    {
        if (mLock)
        {
            LockItemToSelf();
            mLock = false;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Mouse Up.");

            if (mAttachedItem != null)
            {
                mAttachedItem.SetIsBeingDragged(false);

                if (mAttachedItem.GetNearestSlot() == this || mAttachedItem.GetNearestSlot() == null)
                {
                    LockItemToSelf();
                }
                else
                {
                    mAttachedItem.GetNearestSlot().AttachItem(mAttachedItem);
                    DetachItem();
                }
            }
        }
    }

    public SC_ItemOld GetAttachedItem()
    {
        return mAttachedItem;
    }

    public bool AttachItem(SC_ItemOld item)
    {
        if (mAttachedItem == null)
        {
            mAttachedItem = item;
            return LockItemToSelf();
        }

        Debug.Log("WARNING: Attempted to attach item to an already in use slot. (Item: " + item.GetName() + ")");
        return false;
    }

    public bool DetachItem()
    {
        if (mAttachedItem == null)
        {
            Debug.Log("WARNING: Attempted to remove nonexistent item.");
            return false;
        }

        mAttachedItem = null;

        return true;
    }

    public bool LockItemToSelf()
    {
        if (mAttachedItem == null)
        {
            Debug.Log("WARNING: Attempted to lock nonexistent item.");
            return false;
        }

        mAttachedItem.transform.position = transform.position;

        return true;
    }
}
