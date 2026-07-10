using UnityEngine;

public class SC_Item : MonoBehaviour
{
    // --- EFFECT INFO --- //

    public enum Effect
    {
        NONE,
        HEALTH,
        HUNGER,
        ADRENALINE
    }

    [SerializeField] private Effect effect;
    [SerializeField] private float effectStrength;


    // --- UI INFO --- //

    [SerializeField] private GameObject model;
    [SerializeField] private GameObject icon;

    [SerializeField] private string name;
    [SerializeField] private string description;
}
