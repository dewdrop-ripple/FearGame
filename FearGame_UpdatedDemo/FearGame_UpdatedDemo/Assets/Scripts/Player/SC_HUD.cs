using UnityEngine;
using UnityEngine.UI;

public class SC_HUD : MonoBehaviour
{
    [SerializeField] private SC_Player player;

    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider hungerSlider;
    [SerializeField] private Slider staminaSlider;

    private void Update()
    {
        healthSlider.value = player.GetHealth() / player.GetMaxHealth();
        hungerSlider.value = player.GetHunger() / player.GetMaxHunger();
        staminaSlider.value = player.GetStamina() / player.GetMaxStamina();
    }
}
