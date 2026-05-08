using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SC_LootingMenu : MonoBehaviour
{
    // ----- VARIABLES ----- //

    private SC_GameManager mGameManager;

    // Inventory Data
    [SerializeField] private SC_PlayerData mAttachedPlayer;
    [SerializeField] private SC_PlayerMovement mAttachedPlayerMovement;
    [SerializeField] private List<SC_LootingItem> mItemList;
    [SerializeField] private SC_Corpse mTargetBody;

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
        if (mGameManager.GetGameState() == SC_GameManager.GameState.LOOTING)
        {
            mInventoryCanvas.enabled = true;

            for (int i = 0; i < mItemList.Count; i++)
            {
                if (mTargetBody.GetNumItems() <= i)
                {
                    mItemList[i].RemoveItem();
                }
                else
                {
                    mItemList[i].SetItem(mTargetBody.GetItem(i));
                }
            }

            Debug.Log(mItemList[mSelectedIndex].GetText());
            mSelectedItemText.text = mItemList[mSelectedIndex].GetText();
        }
        else
        {
            mInventoryCanvas.enabled = false;
        }
    }

    public void SetBody(SC_Corpse body)
    {
        Debug.Log("Body Set");
        mTargetBody = body;
    }

    public void OpenMenu()
    {
        mAttachedPlayerMovement.SetPaused(true);
        mGameManager.SetGameState(SC_GameManager.GameState.LOOTING);
    }


    // ----- BUTTONS ----- //

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

    public void CollectItem()
    {
        if (mSelectedIndex >= mTargetBody.GetNumItems())
        {
            Debug.Log("Empty Slot");
            return;
        }

        bool success = mAttachedPlayer.AddItem(mItemList[mSelectedIndex].GetItem());

        if (success)
        {
            mTargetBody.RemoveItem(mSelectedIndex);
            mItemList[mSelectedIndex].RemoveItem();
        }
    }

    public void CloseMenu()
    {
        mAttachedPlayerMovement.SetPaused(false);
        mGameManager.SetGameState(SC_GameManager.GameState.PLAYING);
    }
}
