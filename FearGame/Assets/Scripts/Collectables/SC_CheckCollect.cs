using UnityEngine;

public class SC_CheckCollect : MonoBehaviour
{
    // ----- VARIABLES ----- //

    [SerializeField] SC_PlayerData mPlayer;

    private SC_Collectable mTargetedCollectable = null;
    [SerializeField] private SC_Collectable mNullCollectable; 

    private const string M_COLLECTABLE_TAG = "Collectable";


    // ----- FUNCTIONS ----- //

    public void CollectTargeted()
    {
        if (mTargetedCollectable != mNullCollectable)
        {
            mTargetedCollectable.Collect(mPlayer);
        }
    }


    // ----- HANDLE TARGETED OBJECT ----- //

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == M_COLLECTABLE_TAG)
        {
            mTargetedCollectable = collision.gameObject.GetComponent<SC_Collectable>();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == M_COLLECTABLE_TAG)
        {
            mTargetedCollectable = collision.gameObject.GetComponent<SC_Collectable>();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == M_COLLECTABLE_TAG)
        {
            mTargetedCollectable = mNullCollectable;
        }
    }
}
