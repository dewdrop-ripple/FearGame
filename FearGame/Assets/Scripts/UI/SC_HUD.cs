using UnityEngine;
using UnityEngine.UI;

public class SC_HUD : MonoBehaviour
{
    // ----- VARIABLES ----- //

    [SerializeField] private Slider mHungerSlider;
    [SerializeField] private Slider mAdrenalineSlider;
    [SerializeField] private Slider mHealthSlider;
    [SerializeField] private Slider mStaminaSlider;

    [SerializeField] private SC_PlayerData mPlayerData;


    // ----- FUNCTIONS ----- //

    private void Update()
    {
        mHungerSlider.value = mHungerSlider.maxValue - mPlayerData.GetHunger();
        mAdrenalineSlider.value = mAdrenalineSlider.maxValue - mPlayerData.GetAdrenaline();
        mHealthSlider.value = mPlayerData.GetHealth();
        mStaminaSlider.value = mPlayerData.GetStamina();

        mHungerSlider.maxValue = 100;
        mAdrenalineSlider.maxValue = 100;
        mHealthSlider.maxValue = mPlayerData.GetMaxHealth();
        mStaminaSlider.maxValue = mPlayerData.GetMaxStamina();
    }
}
