using TMPro;
using UnityEngine;

public class SC_LootingItem : MonoBehaviour
{
    // ----- VARIABLES ----- //

    [SerializeField] private TextMeshProUGUI mText;
    [SerializeField] private SC_Collectable mItem;
    [SerializeField] private SC_Collectable mNullItem;

    [SerializeField] private int mIndex;

    [SerializeField] private SC_LootingMenu mInventory;


    // ----- FUNCTIONS ----- //

    private void Update()
    {
        if (mItem == mNullItem)
        {
            mText.text = "Empty Slot";
        }
        else
        {
            string text = "Strength " + mItem.GetCollectableStrength();

            switch (mItem.GetCollectableType())
            {
                case SC_Collectable.CollectableType.HEAL:
                    text += " Health Pack";
                    break;

                case SC_Collectable.CollectableType.FOOD:
                    text += " Food";
                    break;

                default:
                    text += " Mystery Item";
                    break;
            }

            mText.text = text;
        }
    }

    public string GetText()
    {
        return mText.text;
    }


    // ----- ITEM INFO ----- //

    public void SetItem(SC_Collectable item)
    {
        mItem = item;
    }

    public void RemoveItem()
    {
        mItem = mNullItem;
    }

    public void Select()
    {
        Debug.Log("Set Index " + mIndex);
        mInventory.SetSelectedIndex(mIndex);
    }

    public SC_Collectable GetItem()
    {
        return mItem;
    }
}
