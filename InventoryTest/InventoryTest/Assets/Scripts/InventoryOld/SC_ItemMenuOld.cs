using System.Collections.Generic;
using UnityEngine;

public class SC_ItemMenuOld : MonoBehaviour
{
    [SerializeField] private List<SC_ItemSlotOld> mItemSlots; // Default: 25
    [SerializeField] private int mRows; // Default: 5
    [SerializeField] private int mColumns; // Default: 5


    // Checks nearest row and column separately, rather than checking every invididual slot, to save time
    public int GetNearestSlot(Vector3 itemPosition, float maxDistance)
    {
        int nearestColumn = 0;
        float nearestColumnDistance = Mathf.Abs(mItemSlots[0].transform.position.x - itemPosition.x);

        for (int i = 1; i < mColumns; i++)
        {
            float targetColumnDistance = Mathf.Abs(mItemSlots[i].transform.position.x - itemPosition.x);

            if (targetColumnDistance < nearestColumnDistance )
            {
                nearestColumnDistance = targetColumnDistance;
                nearestColumn = i;
            }
        }

        int nearestRow = 0;
        float nearestRowDisance = Mathf.Abs(mItemSlots[0].transform.position.y - itemPosition.y);

        for (int i = 1; i < mRows; i += mColumns)
        {
            float targetRowDistance = Mathf.Abs(mItemSlots[i].transform.position.y - itemPosition.y);

            if (targetRowDistance < nearestRowDisance)
            {
                nearestRowDisance = targetRowDistance;
                nearestRow = i;
            }
        }

        int nearestSlot = nearestColumn + (nearestRow * mColumns);

        float nearestSlotDistance = Vector2.Distance(new Vector2(mItemSlots[nearestSlot].transform.position.x, mItemSlots[nearestSlot].transform.position.y),
            new Vector2(itemPosition.x, itemPosition.y));

        if (nearestSlotDistance <= maxDistance)
        {
            return nearestSlot;
        }
        else
        {
            return -1;
        }
    }

    public int GetNextOpenSlot()
    {
        for (int i = 0; i < mItemSlots.Count; i++)
        {
            if (mItemSlots[i].GetAttachedItem() != null)
            {
                return i;
            }
        }

        return -1;
    }

    public bool IsSlotOpen(int slot)
    {
        if (slot < 0 || slot >= mItemSlots.Count) {  return false; }

        return (mItemSlots[slot].GetAttachedItem() == null);
    }

    public bool IsFull()
    {
        for (int i = 0; i < mItemSlots.Count; i++)
        {
            if (mItemSlots[i].GetAttachedItem() == null)
            {
                return false;
            }
        }

        return true;
    }

    public SC_ItemSlotOld GetSlot(int index)
    {
        if (index < 0 || index >= mItemSlots.Count)
        {
            return null;
        }

        return mItemSlots[index];
    }
}
