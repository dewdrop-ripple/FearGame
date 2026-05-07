using Unity.VisualScripting;
using UnityEngine;

public class SC_CharacterDataManager : MonoBehaviour
{
    // ----- VARIABLES ----- //

    // Assigns each possible character a numerical value
    public enum CharacterName
    {
        LILLIAN,
        VIOLET,
        PARKER,
        JANET,
        CANDY,
        JACO,
        BLAKE,
        WILLIAM,
        ANDRES,
        VERA,
        SAKURA,
        NUMBER_OF_CHARACTERS
    }

    [SerializeField] private SC_CharacterData[] mCharacterList;

    // Default character info that applies to all characters
    [SerializeField] private float mBaseAdrenaline;

    [SerializeField] private float mSprintSpeedMultiplier;
    [SerializeField] private float mCrouchSpeedMultiplier;
    
    [SerializeField] private float mBaseSprintStaminaDrain;
    [SerializeField] private float mBaseJumpStaminaDrain;

    [SerializeField] private float mBaseJumpForce;
    [SerializeField] private float mBaseGravity;

    [SerializeField] private float mBaseViewSensitivity;
    [SerializeField] private float mBaseRotationSpeed;

    [SerializeField] private float mBaseHungerDrain;
    [SerializeField] private float mHungerHealthDrain;
    [SerializeField] private float mHungerAdrenalineDrain;
    [SerializeField] private float mHungerAdrenalineThreshhold;

    [SerializeField] private float mEatAdrenalineChange;
    [SerializeField] private float mHealAdrenalineChange;


    // ----- FUNCTIONS ----- //

    // Create character data
    private void Awake()
    {
        // Create a new list
        mCharacterList = new SC_CharacterData[(int) CharacterName.NUMBER_OF_CHARACTERS];

        // Lillian Hendricks
        mCharacterList[(int)CharacterName.LILLIAN] = this.AddComponent<SC_CharacterData>();
        mCharacterList[(int)CharacterName.LILLIAN].SetMaxHealth(75.0f);
        mCharacterList[(int)CharacterName.LILLIAN].SetMaxStamina(60.0f);
        mCharacterList[(int)CharacterName.LILLIAN].SetSpeed(7.0f);
        mCharacterList[(int)CharacterName.LILLIAN].SetStealth(5.0f);
        mCharacterList[(int)CharacterName.LILLIAN].SetReslilience(2.0f);
        mCharacterList[(int)CharacterName.LILLIAN].SetSanity(3.0f);
        mCharacterList[(int)CharacterName.LILLIAN].SetComfortUnderPressure(3.0f);
        mCharacterList[(int)CharacterName.LILLIAN].SetMaxDeaths(15);
        mCharacterList[(int)CharacterName.LILLIAN].SetFearValue(SC_FearData.FearType.PYROPHOBIA, 2.0f);
        mCharacterList[(int)CharacterName.LILLIAN].SetFearValue(SC_FearData.FearType.CLAUSTROPHOBIA, 1.0f);
        mCharacterList[(int)CharacterName.LILLIAN].SetFearValue(SC_FearData.FearType.APHENPHOSMPHOBIA, -1.0f);
        mCharacterList[(int)CharacterName.LILLIAN].SetFearValue(SC_FearData.FearType.AGORAPHOBIA, -2.0f);
        mCharacterList[(int)CharacterName.LILLIAN].SetCharacterName("Lillian Hendricks");
        mCharacterList[(int)CharacterName.LILLIAN].SetDescription("The orphan survivor of a house fire.");

        // Violet LNU
        mCharacterList[(int)CharacterName.VIOLET] = this.AddComponent<SC_CharacterData>();
        mCharacterList[(int)CharacterName.VIOLET].SetMaxHealth(75.0f);
        mCharacterList[(int)CharacterName.VIOLET].SetMaxStamina(60.0f);
        mCharacterList[(int)CharacterName.VIOLET].SetSpeed(4.0f);
        mCharacterList[(int)CharacterName.VIOLET].SetStealth(9.0f);
        mCharacterList[(int)CharacterName.VIOLET].SetReslilience(2.0f);
        mCharacterList[(int)CharacterName.VIOLET].SetSanity(3.0f);
        mCharacterList[(int)CharacterName.VIOLET].SetComfortUnderPressure(4.0f);
        mCharacterList[(int)CharacterName.VIOLET].SetMaxDeaths(13);
        mCharacterList[(int)CharacterName.VIOLET].SetFearValue(SC_FearData.FearType.AQUAPHOBIA, 2.0f);
        mCharacterList[(int)CharacterName.VIOLET].SetFearValue(SC_FearData.FearType.AUTOPHOBIA, 1.0f);
        mCharacterList[(int)CharacterName.VIOLET].SetFearValue(SC_FearData.FearType.ACHLUOPHOBIA, -1.0f);
        mCharacterList[(int)CharacterName.VIOLET].SetFearValue(SC_FearData.FearType.CLAUSTROPHOBIA, -2.0f);
        mCharacterList[(int)CharacterName.VIOLET].SetCharacterName("Violet LNU");
        mCharacterList[(int)CharacterName.VIOLET].SetDescription("An orphan found in a river as a baby.");

        // Parker Draven
        mCharacterList[(int)CharacterName.PARKER] = this.AddComponent<SC_CharacterData>();
        mCharacterList[(int)CharacterName.PARKER].SetMaxHealth(125.0f);
        mCharacterList[(int)CharacterName.PARKER].SetMaxStamina(65.0f);
        mCharacterList[(int)CharacterName.PARKER].SetSpeed(8.0f);
        mCharacterList[(int)CharacterName.PARKER].SetStealth(2.0f);
        mCharacterList[(int)CharacterName.PARKER].SetReslilience(5.0f);
        mCharacterList[(int)CharacterName.PARKER].SetSanity(3.0f);
        mCharacterList[(int)CharacterName.PARKER].SetComfortUnderPressure(2.0f);
        mCharacterList[(int)CharacterName.PARKER].SetMaxDeaths(20);
        mCharacterList[(int)CharacterName.PARKER].SetFearValue(SC_FearData.FearType.AGORAPHOBIA, 2.0f);
        mCharacterList[(int)CharacterName.PARKER].SetFearValue(SC_FearData.FearType.PHONOPHOBIA, 1.0f);
        mCharacterList[(int)CharacterName.PARKER].SetFearValue(SC_FearData.FearType.AUTOPHOBIA, -1.0f);
        mCharacterList[(int)CharacterName.PARKER].SetFearValue(SC_FearData.FearType.CATEGELOPHOBIA, -2.0f);
        mCharacterList[(int)CharacterName.PARKER].SetCharacterName("Parker Draven");
        mCharacterList[(int)CharacterName.PARKER].SetDescription("A mass shooting survivor.");

        // Janet Nelson
        mCharacterList[(int)CharacterName.JANET] = this.AddComponent<SC_CharacterData>();
        mCharacterList[(int)CharacterName.JANET].SetMaxHealth(175.0f);
        mCharacterList[(int)CharacterName.JANET].SetMaxStamina(45.0f);
        mCharacterList[(int)CharacterName.JANET].SetSpeed(4.0f);
        mCharacterList[(int)CharacterName.JANET].SetStealth(5.0f);
        mCharacterList[(int)CharacterName.JANET].SetReslilience(6.0f);
        mCharacterList[(int)CharacterName.JANET].SetSanity(10.0f);
        mCharacterList[(int)CharacterName.JANET].SetComfortUnderPressure(10.0f);
        mCharacterList[(int)CharacterName.JANET].SetMaxDeaths(20);
        mCharacterList[(int)CharacterName.JANET].SetFearValue(SC_FearData.FearType.CLAUSTROPHOBIA, 2.0f);
        mCharacterList[(int)CharacterName.JANET].SetFearValue(SC_FearData.FearType.ACHLUOPHOBIA, 1.0f);
        mCharacterList[(int)CharacterName.JANET].SetFearValue(SC_FearData.FearType.AQUAPHOBIA, -1.0f);
        mCharacterList[(int)CharacterName.JANET].SetFearValue(SC_FearData.FearType.APHENPHOSMPHOBIA, -2.0f);
        mCharacterList[(int)CharacterName.JANET].SetCharacterName("Janet Nelson");
        mCharacterList[(int)CharacterName.JANET].SetDescription("A kidnapped hippie.");

        // Candy Caballero
        mCharacterList[(int)CharacterName.CANDY] = this.AddComponent<SC_CharacterData>();
        mCharacterList[(int)CharacterName.CANDY].SetMaxHealth(125.0f);
        mCharacterList[(int)CharacterName.CANDY].SetMaxStamina(65.0f);
        mCharacterList[(int)CharacterName.CANDY].SetSpeed(2.0f);
        mCharacterList[(int)CharacterName.CANDY].SetStealth(2.0f);
        mCharacterList[(int)CharacterName.CANDY].SetReslilience(4.0f);
        mCharacterList[(int)CharacterName.CANDY].SetSanity(4.0f);
        mCharacterList[(int)CharacterName.CANDY].SetComfortUnderPressure(8.0f);
        mCharacterList[(int)CharacterName.CANDY].SetMaxDeaths(15);
        mCharacterList[(int)CharacterName.CANDY].SetFearValue(SC_FearData.FearType.APHENPHOSMPHOBIA, 2.0f);
        mCharacterList[(int)CharacterName.CANDY].SetFearValue(SC_FearData.FearType.CATEGELOPHOBIA, 1.0f);
        mCharacterList[(int)CharacterName.CANDY].SetFearValue(SC_FearData.FearType.PHONOPHOBIA, -1.0f);
        mCharacterList[(int)CharacterName.CANDY].SetFearValue(SC_FearData.FearType.AUTOPHOBIA, -2.0f);
        mCharacterList[(int)CharacterName.CANDY].SetCharacterName("Candy Caballero");
        mCharacterList[(int)CharacterName.CANDY].SetDescription("A former prostitute.");

        // Jaco Carver
        mCharacterList[(int)CharacterName.JACO] = this.AddComponent<SC_CharacterData>();
        mCharacterList[(int)CharacterName.JACO].SetMaxHealth(150.0f);
        mCharacterList[(int)CharacterName.JACO].SetMaxStamina(35.0f);
        mCharacterList[(int)CharacterName.JACO].SetSpeed(7.0f);
        mCharacterList[(int)CharacterName.JACO].SetStealth(7.0f);
        mCharacterList[(int)CharacterName.JACO].SetReslilience(2.0f);
        mCharacterList[(int)CharacterName.JACO].SetSanity(1.0f);
        mCharacterList[(int)CharacterName.JACO].SetComfortUnderPressure(3.0f);
        mCharacterList[(int)CharacterName.JACO].SetMaxDeaths(20);
        mCharacterList[(int)CharacterName.JACO].SetFearValue(SC_FearData.FearType.INSECTOPHOBIA, 2.0f);
        mCharacterList[(int)CharacterName.JACO].SetFearValue(SC_FearData.FearType.APHENPHOSMPHOBIA, 1.0f);
        mCharacterList[(int)CharacterName.JACO].SetFearValue(SC_FearData.FearType.CATEGELOPHOBIA, -1.0f);
        mCharacterList[(int)CharacterName.JACO].SetFearValue(SC_FearData.FearType.ACROPHOBIA, -2.0f);
        mCharacterList[(int)CharacterName.JACO].SetCharacterName("Jaco Carver");
        mCharacterList[(int)CharacterName.JACO].SetDescription("An escaped slave.");

        // Blake Fox
        mCharacterList[(int)CharacterName.BLAKE] = this.AddComponent<SC_CharacterData>();
        mCharacterList[(int)CharacterName.BLAKE].SetMaxHealth(175.0f);
        mCharacterList[(int)CharacterName.BLAKE].SetMaxStamina(50.0f);
        mCharacterList[(int)CharacterName.BLAKE].SetSpeed(1.0f);
        mCharacterList[(int)CharacterName.BLAKE].SetStealth(1.0f);
        mCharacterList[(int)CharacterName.BLAKE].SetReslilience(2.0f);
        mCharacterList[(int)CharacterName.BLAKE].SetSanity(1.0f);
        mCharacterList[(int)CharacterName.BLAKE].SetComfortUnderPressure(1.0f);
        mCharacterList[(int)CharacterName.BLAKE].SetMaxDeaths(20);
        mCharacterList[(int)CharacterName.BLAKE].SetFearValue(SC_FearData.FearType.ACROPHOBIA, 2.0f);
        mCharacterList[(int)CharacterName.BLAKE].SetFearValue(SC_FearData.FearType.AQUAPHOBIA, 1.0f);
        mCharacterList[(int)CharacterName.BLAKE].SetFearValue(SC_FearData.FearType.INSECTOPHOBIA, -1.0f);
        mCharacterList[(int)CharacterName.BLAKE].SetFearValue(SC_FearData.FearType.ACHLUOPHOBIA, -2.0f);
        mCharacterList[(int)CharacterName.BLAKE].SetCharacterName("Blake Fox");
        mCharacterList[(int)CharacterName.BLAKE].SetDescription("A former stuntman.");

        // William Smith
        mCharacterList[(int)CharacterName.WILLIAM] = this.AddComponent<SC_CharacterData>();
        mCharacterList[(int)CharacterName.WILLIAM].SetMaxHealth(200.0f);
        mCharacterList[(int)CharacterName.WILLIAM].SetMaxStamina(50.0f);
        mCharacterList[(int)CharacterName.WILLIAM].SetSpeed(4.0f);
        mCharacterList[(int)CharacterName.WILLIAM].SetStealth(2.0f);
        mCharacterList[(int)CharacterName.WILLIAM].SetReslilience(7.0f);
        mCharacterList[(int)CharacterName.WILLIAM].SetSanity(3.0f);
        mCharacterList[(int)CharacterName.WILLIAM].SetComfortUnderPressure(9.0f);
        mCharacterList[(int)CharacterName.WILLIAM].SetMaxDeaths(25);
        mCharacterList[(int)CharacterName.WILLIAM].SetFearValue(SC_FearData.FearType.PHONOPHOBIA, 2.0f);
        mCharacterList[(int)CharacterName.WILLIAM].SetFearValue(SC_FearData.FearType.PYROPHOBIA, 1.0f);
        mCharacterList[(int)CharacterName.WILLIAM].SetFearValue(SC_FearData.FearType.ACROPHOBIA, -1.0f);
        mCharacterList[(int)CharacterName.WILLIAM].SetFearValue(SC_FearData.FearType.AQUAPHOBIA, -2.0f);
        mCharacterList[(int)CharacterName.WILLIAM].SetCharacterName("William Smith");
        mCharacterList[(int)CharacterName.WILLIAM].SetDescription("A former firefighter.");

        // Andres Beckett
        mCharacterList[(int)CharacterName.ANDRES] = this.AddComponent<SC_CharacterData>();
        mCharacterList[(int)CharacterName.ANDRES].SetMaxHealth(200.0f);
        mCharacterList[(int)CharacterName.ANDRES].SetMaxStamina(60.0f);
        mCharacterList[(int)CharacterName.ANDRES].SetSpeed(3.0f);
        mCharacterList[(int)CharacterName.ANDRES].SetStealth(6.0f);
        mCharacterList[(int)CharacterName.ANDRES].SetReslilience(4.0f);
        mCharacterList[(int)CharacterName.ANDRES].SetSanity(2.0f);
        mCharacterList[(int)CharacterName.ANDRES].SetComfortUnderPressure(5.0f);
        mCharacterList[(int)CharacterName.ANDRES].SetMaxDeaths(20);
        mCharacterList[(int)CharacterName.ANDRES].SetFearValue(SC_FearData.FearType.ACHLUOPHOBIA, 2.0f);
        mCharacterList[(int)CharacterName.ANDRES].SetFearValue(SC_FearData.FearType.INSECTOPHOBIA, 1.0f);
        mCharacterList[(int)CharacterName.ANDRES].SetFearValue(SC_FearData.FearType.AGORAPHOBIA, -1.0f);
        mCharacterList[(int)CharacterName.ANDRES].SetFearValue(SC_FearData.FearType.PYROPHOBIA, -2.0f);
        mCharacterList[(int)CharacterName.ANDRES].SetCharacterName("Andres Beckett");
        mCharacterList[(int)CharacterName.ANDRES].SetDescription("One half of a camping couple.");

        // Vera Beckett
        mCharacterList[(int)CharacterName.VERA] = this.AddComponent<SC_CharacterData>();
        mCharacterList[(int)CharacterName.VERA].SetMaxHealth(175.0f);
        mCharacterList[(int)CharacterName.VERA].SetMaxStamina(75.0f);
        mCharacterList[(int)CharacterName.VERA].SetSpeed(3.0f);
        mCharacterList[(int)CharacterName.VERA].SetStealth(8.0f);
        mCharacterList[(int)CharacterName.VERA].SetReslilience(2.0f);
        mCharacterList[(int)CharacterName.VERA].SetSanity(4.0f);
        mCharacterList[(int)CharacterName.VERA].SetComfortUnderPressure(8.0f);
        mCharacterList[(int)CharacterName.VERA].SetMaxDeaths(25);
        mCharacterList[(int)CharacterName.VERA].SetFearValue(SC_FearData.FearType.AUTOPHOBIA, 2.0f);
        mCharacterList[(int)CharacterName.VERA].SetFearValue(SC_FearData.FearType.ACROPHOBIA, 1.0f);
        mCharacterList[(int)CharacterName.VERA].SetFearValue(SC_FearData.FearType.PYROPHOBIA, -1.0f);
        mCharacterList[(int)CharacterName.VERA].SetFearValue(SC_FearData.FearType.INSECTOPHOBIA, -2.0f);
        mCharacterList[(int)CharacterName.VERA].SetCharacterName("Vera Beckett");
        mCharacterList[(int)CharacterName.VERA].SetDescription("One half of a camping couple.");

        // Sakura Mori
        mCharacterList[(int)CharacterName.SAKURA] = this.AddComponent<SC_CharacterData>();
        mCharacterList[(int)CharacterName.SAKURA].SetMaxHealth(100.0f);
        mCharacterList[(int)CharacterName.SAKURA].SetMaxStamina(35.0f);
        mCharacterList[(int)CharacterName.SAKURA].SetSpeed(2.0f);
        mCharacterList[(int)CharacterName.SAKURA].SetStealth(2.0f);
        mCharacterList[(int)CharacterName.SAKURA].SetReslilience(2.0f);
        mCharacterList[(int)CharacterName.SAKURA].SetSanity(10.0f);
        mCharacterList[(int)CharacterName.SAKURA].SetComfortUnderPressure(9.0f);
        mCharacterList[(int)CharacterName.SAKURA].SetMaxDeaths(15);
        mCharacterList[(int)CharacterName.SAKURA].SetFearValue(SC_FearData.FearType.CATEGELOPHOBIA, 2.0f);
        mCharacterList[(int)CharacterName.SAKURA].SetFearValue(SC_FearData.FearType.AGORAPHOBIA, 1.0f);
        mCharacterList[(int)CharacterName.SAKURA].SetFearValue(SC_FearData.FearType.CLAUSTROPHOBIA, -1.0f);
        mCharacterList[(int)CharacterName.SAKURA].SetFearValue(SC_FearData.FearType.PHONOPHOBIA, -2.0f);
        mCharacterList[(int)CharacterName.SAKURA].SetCharacterName("Sakura Mori");
        mCharacterList[(int)CharacterName.SAKURA].SetDescription("A failed comedian.");
    }


    // ----- CHARACTERS ----- //

    public SC_CharacterData GetCharacterData(CharacterName name)
    {
        return mCharacterList[(int) name];
    }


    // ----- DEFAULT DATA ----- //

    public float GetBaseAdrenaline()
    {
        return mBaseAdrenaline;
    }

    public float GetSprintSpeedMultiplier()
    {
        return mSprintSpeedMultiplier;
    }

    public float GetCrouchSpeedMultiplier()
    {
        return mCrouchSpeedMultiplier;
    }    

    public float GetBaseSprintStaminaDrain()
    {
        return mBaseSprintStaminaDrain;
    }

    public float GetBaseJumpStaminaDrain()
    {
        return mBaseJumpStaminaDrain;
    }

    public float GetBaseJumpForce()
    {
        return mBaseJumpForce;
    }

    public float GetBaseGravity()
    {
        return mBaseGravity;
    }

    public float GetBaseViewSensitivity()
    {
        return mBaseViewSensitivity;
    }

    public float GetBaseRotationSpeed()
    {
        return mBaseRotationSpeed;
    }

    public float GetBaseHungerDrain()
    {
        return mBaseHungerDrain;
    }

    public float GetHungerHealthDrain()
    {
        return mHungerAdrenalineThreshhold;
    }

    public float GetHungerAdrenalineDrain()
    {
        return mHungerAdrenalineDrain;
    }

    public float GetHungerAdrenalineThreshhold()
    {
        return mHungerAdrenalineThreshhold;
    }

    public float GetEatAdrenalineChange()
    {
        return mEatAdrenalineChange;
    }

    public float GetHealAdrenalineChange()
    {
        return mHealAdrenalineChange;
    }
}
