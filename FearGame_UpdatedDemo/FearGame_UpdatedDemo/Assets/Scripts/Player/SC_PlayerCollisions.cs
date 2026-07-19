using UnityEngine;

public class SC_PlayerCollisions : MonoBehaviour
{
    [SerializeField] private SC_Player player;

    private void OnTriggerStay(Collider other)
    { 
        if (other.tag == "Damage")
        {
            player.TakeDamage(other.GetComponent<SC_Damage>().GetDamage());
        }
    }
}
