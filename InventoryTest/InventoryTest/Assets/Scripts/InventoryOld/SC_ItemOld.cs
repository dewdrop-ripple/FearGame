using Unity.VisualScripting;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SC_ItemOld : MonoBehaviour
{
    public enum EffectType 
    {
        NONE,
        HEALTH,
        HUNGER,
        ADRENALINE
    }

    [SerializeField] private GameObject mModel;

    [SerializeField] private GameObject mIcon;
    [SerializeField] private string mName;
    [SerializeField] private string mDescription;

    [SerializeField] private EffectType mEffectType;
    [SerializeField] private float mEffectStrength;

    [SerializeField] private bool mIsStored;

    [SerializeField] private bool mIsBeingDragged;
    [SerializeField] private SC_ItemMenuOld mTargetMenu;

    [SerializeField] private float mMaxSlotClipDistance;


    private void Update()
    {
        if (mIsBeingDragged && mTargetMenu != null)
        {
            Debug.Log("Being Dragged");

            Vector3 targetPos = new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, 0);
            int nearestSlot = mTargetMenu.GetNearestSlot(targetPos, mMaxSlotClipDistance);

            if (nearestSlot == -1)
            {
                transform.position = targetPos;
            }
            else
            {
                transform.position = mTargetMenu.GetSlot(nearestSlot).transform.position;
            }
        }
    }

    public bool AddToMenu(SC_ItemMenuOld targetMenu)
    {
        if (mIsStored)
        {
            Debug.Log("WARNING: Cannot store item \'" + mName + "\' as item is already stored.");
            return false;
        }

        if (targetMenu.IsFull())
        {
            Debug.Log("WARNING: Cannot store item \'" + mName + "\' as storage container is full.");
            return false;
        }

        int targetIndex = targetMenu.GetNextOpenSlot();
        SC_ItemSlotOld targetSlot = targetMenu.GetSlot(targetIndex);

        bool success = targetSlot.AttachItem(this);

        if (!success)
        {
            Debug.Log("WARNING: Failed to attach item \'" + mName + "\'");
            return false;
        }

        SetStoredState(true);

        return true;
    }

    public void Drop()
    {
        SetStoredState(false);
    }

    private void SetStoredState(bool isStored)
    {
        mIsStored = isStored;

        if (mIsStored)
        {
            mModel.GetComponent<Renderer>().enabled = false;
            mModel.GetComponent<Collider>().enabled = false;
        }
        else
        {
            mModel.GetComponent<Renderer>().enabled = true;
            mModel.GetComponent<Collider>().enabled = true;
        }
    }

    public void SetUIVisibility(bool isVisible)
    {
        if (isVisible)
        {
            mIcon.GetComponent<Renderer>().enabled = true;
            mIcon.GetComponent<Collider>().enabled = true;
        }
        else
        {
            mIcon.GetComponent<Renderer>().enabled = false;
            mIcon.GetComponent<Collider>().enabled = false;
        }
    }

    public string GetName()
    {
        return mName;
    }

    public string GetDescription()
    {
        return mDescription;
    }

    public EffectType GetEffectType()
    {
        return mEffectType;
    }

    public float GetEffectStrength()
    {
        return mEffectStrength;
    }

    public void SetIsBeingDragged(bool isBeingDragged)
    {
        mIsBeingDragged = isBeingDragged;
    }

    public void SetTargetMenu(SC_ItemMenuOld targetMenu)
    {
        mTargetMenu = targetMenu;
    }

    public SC_ItemSlotOld GetNearestSlot()
    {
        return mTargetMenu.GetSlot(mTargetMenu.GetNearestSlot(transform.position, mMaxSlotClipDistance));
    }
}
