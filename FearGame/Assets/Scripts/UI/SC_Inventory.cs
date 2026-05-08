using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SC_Inventory : MonoBehaviour
{
    // ----- VARIABLES ----- //

    private SC_GameManager mGameManager;

    // Inventory Data
    [SerializeField] private SC_PlayerData mAttachedPlayer;
    [SerializeField] private SC_PlayerMovement mAttachedPlayerMovement;
    [SerializeField] private List<SC_InventoryItem> mItemList;
    [SerializeField] private GameObject mDropSpot;

    // UI Data
    [SerializeField] private Canvas mInventoryCanvas;

    [SerializeField] private TextMeshProUGUI mSelectedItemText;
    private int mSelectedIndex = 0;


    // ----- FUNCTIONS ----- //

    private void Start()
    {
        mGameManager = FindAnyObjectByType<SC_GameManager>();
    }

    private void Update()
    {
        if (mGameManager.GetGameState() == SC_GameManager.GameState.INVENTORY)
        {
            mInventoryCanvas.enabled = true;

            for (int i = 0; i < mItemList.Count; i++)
            {
                if (mAttachedPlayer.GetNumItems() <= i)
                {
                    mItemList[i].RemoveItem();
                }
                else 
                {
                    mItemList[i].SetItem(mAttachedPlayer.GetItem(i));
                }
            }

            mSelectedItemText.text = mItemList[mSelectedIndex].GetText();
        }
        else
        {
            mInventoryCanvas.enabled = false;
        }
    }

    public void SetSelectedIndex(int index)
    {
        mSelectedIndex = index;

        if (mSelectedIndex < 0)
        {
            mSelectedIndex = 0;
        }
        else if (mSelectedIndex > mItemList.Count - 1)
        {
            mSelectedIndex = mItemList.Count - 1;
        }
    }


    // ----- BUTTONS ----- //

    public void UseItem()
    {
        bool success = mItemList[mSelectedIndex].GetItem().UseItem(mAttachedPlayer);

        if (success)
        {
            mAttachedPlayer.RemoveItem(mItemList[mSelectedIndex].GetItem());
        }
    }

    public void DropItem()
    {
        mItemList[mSelectedIndex].GetItem().gameObject.transform.position = mDropSpot.transform.position;

        mItemList[mSelectedIndex].GetItem().gameObject.GetComponent<Renderer>().enabled = true;
        mItemList[mSelectedIndex].GetItem().gameObject.GetComponent<Collider>().enabled = true;
        mItemList[mSelectedIndex].GetItem().gameObject.GetComponent<Rigidbody>().detectCollisions = true;

        mAttachedPlayer.RemoveItemWithoutDestroying(mItemList[mSelectedIndex].GetItem());
    }

    public void CloseInventory()
    {
        mAttachedPlayerMovement.SetPaused(false);
        mGameManager.SetGameState(SC_GameManager.GameState.PLAYING);
    }
}
