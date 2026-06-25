using UnityEngine;

public class SC_CheckCollect : MonoBehaviour
{
    // ----- VARIABLES ----- //

    [SerializeField] SC_PlayerData mPlayer;
    [SerializeField] SC_LootingMenu mLootingMenu;
    [SerializeField] SC_TextBox mTextBox;

    [SerializeField] private SC_Collectable mTargetedCollectable = null;
    [SerializeField] private SC_Corpse mTargetedBody = null;
    [SerializeField] private SC_NPC mTargetedNPC = null;
    [SerializeField] private SC_Collectable mNullCollectable; 

    private const string M_COLLECTABLE_TAG = "Collectable";
    private const string M_BODY_TAG = "Corpse";
    private const string M_NPC_TAG = "NPC";


    // ----- FUNCTIONS ----- //

    private void Start()
    {
        mTargetedCollectable = mNullCollectable;
    }

    public void CollectTargeted()
    {
        if (mTargetedCollectable != mNullCollectable)
        {
            mTargetedCollectable.Collect(mPlayer);
            Debug.Log("Collect Item");
        }
        else if (mTargetedBody != null)
        {
            mLootingMenu.SetBody(mTargetedBody);
            mLootingMenu.OpenMenu();
            Debug.Log("Loot Body");
        }
        else if (mTargetedNPC != null)
        {
            mTextBox.SetNPC(mTargetedNPC);
            mTextBox.OpenMenu();
            Debug.Log("Talk to NPC");
        }
    }


    // ----- HANDLE TARGETED OBJECT ----- //

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == M_COLLECTABLE_TAG)
        {
            mTargetedCollectable = collision.gameObject.GetComponent<SC_Collectable>();
            Debug.Log("Collectable Targeted");
        }
        else if (collision.gameObject.tag == M_BODY_TAG)
        {
            mTargetedBody = collision.gameObject.GetComponent<SC_Corpse>();
            Debug.Log("Body Targeted");
        }
        else if (collision.gameObject.tag == M_NPC_TAG)
        {
            mTargetedNPC = collision.gameObject.GetComponent<SC_NPC>();
            Debug.Log("NPC Targeted");
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == M_COLLECTABLE_TAG)
        {
            mTargetedCollectable = collision.gameObject.GetComponent<SC_Collectable>();
            Debug.Log("Collectable Targeted");
        }
        else if (collision.gameObject.tag == M_BODY_TAG)
        {
            mTargetedBody = collision.gameObject.GetComponent<SC_Corpse>();
            Debug.Log("Body Targeted");
        }
        else if (collision.gameObject.tag == M_NPC_TAG)
        {
            mTargetedNPC = collision.gameObject.GetComponent<SC_NPC>();
            Debug.Log("NPC Targeted");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == M_COLLECTABLE_TAG)
        {
            mTargetedCollectable = mNullCollectable;
            Debug.Log("Collectable Untargeted");
        }
        else if (collision.gameObject.tag == M_BODY_TAG)
        {
            mTargetedBody = null;
            Debug.Log("Body Untargeted");
        }
        else if (collision.gameObject.tag == M_NPC_TAG)
        {
            mTargetedNPC = null;
            Debug.Log("NPC Untargeted");
        }
    }
}
