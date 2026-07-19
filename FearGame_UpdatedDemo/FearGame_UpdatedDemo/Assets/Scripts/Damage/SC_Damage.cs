using UnityEngine;

public class SC_Damage : MonoBehaviour
{
    [SerializeField] private float damage;

    public float GetDamage()
    {
        return damage;
    }
}
