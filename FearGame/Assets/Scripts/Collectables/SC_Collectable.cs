using UnityEngine;

public class SC_Collectable : MonoBehaviour
{
    // ----- VARIABLES ----- //

    public enum CollectableType
    {
        HEAL,
        FOOD
    }

    [SerializeField] private CollectableType mType;
    [SerializeField] private float mEffectStrength;


    // ----- FUNCTIONS ----- //

    public void Collect(SC_PlayerData collector)
    {
        switch (mType)
        {
            case CollectableType.HEAL:
                collector.SetHealth(collector.GetHealth() + mEffectStrength);
                break;

            case CollectableType.FOOD:
                collector.SetHunger(collector.GetHunger() - mEffectStrength);
                break;

            default:
                Debug.Log("WARNING: Unknown collectable type.");
                break;
        }

        Destroy(gameObject);
    }
}
