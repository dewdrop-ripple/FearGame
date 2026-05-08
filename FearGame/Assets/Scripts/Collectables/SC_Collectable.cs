using UnityEngine;

public class SC_Collectable : MonoBehaviour
{
    // ----- VARIABLES ----- //

    public enum CollectableType
    {
        HEAL,
        FOOD,
        NONE
    }

    [SerializeField] private CollectableType mType;
    [SerializeField] private float mEffectStrength;


    // ----- FUNCTIONS ----- //

    public void Collect(SC_PlayerData collector)
    {
        if (collector.AddItem(this))
        {
            gameObject.GetComponent<Renderer>().enabled = false;
            gameObject.GetComponent<Collider>().enabled = false;
            gameObject.GetComponent<Rigidbody>().detectCollisions = false;
        }
    }

    public bool UseItem(SC_PlayerData user)
    {
        switch (mType)
        {
            case CollectableType.HEAL:
                user.SetHealth(user.GetHealth() + mEffectStrength);
                return true;
                break;

            case CollectableType.FOOD:
                user.SetHunger(user.GetHunger() - mEffectStrength);
                return true;
                break;

            default:
                Debug.Log("WARNING: Unknown collectable type.");
                return false;
                break;
        }
    }

    public CollectableType GetCollectableType()
    {
        return mType;
    }

    public float GetCollectableStrength()
    {
        return mEffectStrength;
    }

    /*
    // ----- OPERATOR OVERLOADS ----- //
    // Used to sort the inventory

    public static bool operator < (SC_Collectable first, SC_Collectable second)
    {
        if (first.GetCollectableType() < second.GetCollectableType())
        {
            return true;
        }

        if (first.GetCollectableType() > second.GetCollectableType())
        {
            return false;
        }

        return first.GetCollectableStrength() < second.GetCollectableStrength();
    }

    public static bool operator > (SC_Collectable first, SC_Collectable second)
    {
        if (first.GetCollectableType() > second.GetCollectableType())
        {
            return true;
        }

        if (first.GetCollectableType() < second.GetCollectableType())
        {
            return false;
        }

        return first.GetCollectableStrength() > second.GetCollectableStrength();
    }

    public static bool operator <= (SC_Collectable first, SC_Collectable second)
    {
        if (first.GetCollectableType() < second.GetCollectableType())
        {
            return true;
        }

        if (first.GetCollectableType() == second.GetCollectableType())
        {
            return first.GetCollectableStrength() <= second.GetCollectableStrength();
        }

        return false;
    }

    public static bool operator >= (SC_Collectable first, SC_Collectable second)
    {
        if (first.GetCollectableType() > second.GetCollectableType())
        {
            return true;
        }

        if (first.GetCollectableType() == second.GetCollectableType())
        {
            return first.GetCollectableStrength() >= second.GetCollectableStrength();
        }

        return false;
    }

    public static bool operator == (SC_Collectable first, SC_Collectable second)
    {
        return first.GetCollectableType() == second.GetCollectableType() &&
            first.GetCollectableStrength() == second.GetCollectableStrength();
    }

    public static bool operator != (SC_Collectable first, SC_Collectable second)
    {
        return first.GetCollectableType() != second.GetCollectableType() ||
            first.GetCollectableStrength() != second.GetCollectableStrength();
    }
    */
}
