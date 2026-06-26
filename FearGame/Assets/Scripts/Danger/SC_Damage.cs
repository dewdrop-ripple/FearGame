using UnityEngine;

public class SC_Damage : MonoBehaviour
{
    // ----- VARIABLES ----- //

    [SerializeField] private float mDamage;
    [SerializeField] private float mCooldown;

    private float mTimer;


    // ----- FUNCTIONS ----- //

    private void Update()
    {
        if (mTimer > 0)
        {
            mTimer -= Time.deltaTime;
        }
    }

    public void SetDamage(float damage)
    { 
        mDamage = damage; 
    }

    public float GetDamage()
    {
        return mDamage;
    }

    public void StartCooldown()
    {
        mTimer = mCooldown;
    }

    public void CancelCooldown()
    {
        mTimer = 0;
    }

    public bool CanDealDamage()
    {
        return mTimer <= 0.0f;
    }
}
