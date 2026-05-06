using UnityEngine;

public class SC_ZoneData : MonoBehaviour
{
    // ----- VARIABLES ----- //

    [SerializeField] private SC_FearData.FearType mFearType;
    [SerializeField] private MeshRenderer mVisualArea;

    [SerializeField] private bool mIsVisible;

    [SerializeField] private Material[] mFearTypeMaterials;
    [SerializeField] private Material mInvisibleMaterial;


    // ----- FUNCTIONS ----- //

    private void Update()
    { 
        if (mIsVisible)
        {
            if (mFearType == SC_FearData.FearType.SAFE)
            {
                mVisualArea.material = mFearTypeMaterials[11];
            }
            else
            {
                mVisualArea.material = mFearTypeMaterials[(int) mFearType];
            }
        }
        else
        {
            mVisualArea.material = mInvisibleMaterial;
        }
    }

    public SC_FearData.FearType GetFearType()
    {
        return mFearType;
    }

    public void SetVisibility(bool isVisible)
    {
        mIsVisible = isVisible;
    }
}
