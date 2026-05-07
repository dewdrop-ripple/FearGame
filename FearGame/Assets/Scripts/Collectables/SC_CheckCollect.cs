using UnityEngine;

public class SC_CheckCollect : MonoBehaviour
{
    // ----- VARIABLES ----- //

    [SerializeField] SC_PlayerData mPlayer;

    private SC_Collectable mTargetedCollectable = null;

    private const string M_COLLECTABLE_TAG = "Collectable";


    // ----- FUNCTIONS ----- //

    public void CollectTargeted()
    {
        if (mTargetedCollectable != null)
        {
            Debug.Log("Collecting Item");
            mTargetedCollectable.Collect(mPlayer);
        }
        else
        {
            Debug.Log("Item Not Collected");
        }
    }


    // ----- HANDLE TARGETED OBJECT ----- //

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision Enter");

        if (collision.gameObject.tag == M_COLLECTABLE_TAG)
        {
            Debug.Log("Item Targeted");
            mTargetedCollectable = collision.gameObject.GetComponent<SC_Collectable>();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("Collision Stay");

        if (collision.gameObject.tag == M_COLLECTABLE_TAG)
        {
            Debug.Log("Item Targeted");
            mTargetedCollectable = collision.gameObject.GetComponent<SC_Collectable>();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("Collision Exit");

        if (collision.gameObject.tag == M_COLLECTABLE_TAG)
        {
            Debug.Log("Item Untargeted");
            mTargetedCollectable = null;
        }
    }
}
