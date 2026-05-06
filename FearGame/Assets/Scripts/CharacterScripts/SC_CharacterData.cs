using UnityEngine;
using static SC_FearData;

public class SC_CharacterData : MonoBehaviour
{
    // ----- VARIABLES ----- //

    // How good are the character's stats?
    public enum CharacterDifficulty
    {
        NONE,
        HARD,
        MEDIUM,
        EASY
    }

    // CHARACTER STATS
    private const float DEFAULT_DATA_VALUE = 0.0f;

    [SerializeField] private float mMaxHealth;
    [SerializeField] private float mMaxStamina;
    [SerializeField] private float mSpeed;
    [SerializeField] private float mStealth;
    [SerializeField] private float mResilience;
    [SerializeField] private float mSanity;
    [SerializeField] private float mComfortUnderPressure;
    [SerializeField] private int mMaxNumberOfDeaths;

    [SerializeField] private float[] mFearList;

    // DESCRIPTION DATA
    private const string DEFAULT_SRTING_VALUE = "";

    private string mCharacterName;
    private string mCharacterDescription;


    // ----- FUNCTIONS ----- //

    // Creates a new list of a character's fears and sets all values to the default 
    // Sets all other data to a default empty value of 0
    private void Awake()
    {
        // Default fear list
        mFearList = new float[(int) FearType.NUMBER_OF_FEARS];

        for (int i = 0; i < mFearList.Length; i++)
        {
            mFearList[i] = DEFAULT_DATA_VALUE;
        }

        // Default stats
        mMaxHealth = DEFAULT_DATA_VALUE;
        mMaxStamina = DEFAULT_DATA_VALUE;
        mSpeed = DEFAULT_DATA_VALUE;
        mStealth = DEFAULT_DATA_VALUE;
        mResilience = DEFAULT_DATA_VALUE;
        mSanity = DEFAULT_DATA_VALUE;
        mComfortUnderPressure = DEFAULT_DATA_VALUE ;
        mMaxNumberOfDeaths = (int) DEFAULT_DATA_VALUE;

        // Default character info
        mCharacterName = DEFAULT_SRTING_VALUE;
        mCharacterDescription = DEFAULT_SRTING_VALUE;
    }


    // ----- STAT UTILITY FUNCTIONS ----- //

    // Value Limits:
    //   Maximum Health (50 - 250)
    //   Maximum Stamina (25 - 100)
    //   Speed (1 - 10)
    //   Stealth (1 - 10)
    //   Resilience(1 - 10)
    //   Sanity (1 - 10)
    //   Comfort Under Pressure (1 - 10)
    //   Maximum Number of Deaths (10 - 30)

    public void SetMaxHealth(float maxHealth)
    {
        const float MAXIMUM = 250.0f;
        const float MINIMUM = 50.0f;

        mMaxHealth = maxHealth;

        if (mMaxHealth < MINIMUM)
        {
            mMaxHealth = MINIMUM;
        }
        else if (mMaxHealth > MAXIMUM)
        {
            mMaxHealth = MAXIMUM;
        }
    }

    public void SetMaxStamina(float maxStamina)
    {
        const float MAXIMUM = 100.0f;
        const float MINIMUM = 25.0f;

        mMaxStamina = maxStamina;

        if (mMaxStamina < MINIMUM)
        {
            mMaxStamina = MINIMUM;
        }
        else if (mMaxStamina > MAXIMUM)
        {
            mMaxStamina = MAXIMUM;
        }
    }

    public void SetSpeed(float speed)
    {
        const float MAXIMUM = 10.0f;
        const float MINIMUM = 1.0f;

        mSpeed = speed;

        if (mSpeed < MINIMUM)
        {
            mSpeed = MINIMUM;
        }
        else if (mSpeed > MAXIMUM)
        {
            mSpeed = MAXIMUM;
        }
    }

    public void SetStealth(float stealth)
    {
        const float MAXIMUM = 10.0f;
        const float MINIMUM = 1.0f;

        mStealth = stealth;

        if (mStealth < MINIMUM)
        {
            mStealth = MINIMUM;
        }
        else if (mStealth > MAXIMUM)
        {
            mStealth = MAXIMUM;
        }
    }

    public void SetReslilience(float resilience)
    {
        const float MAXIMUM = 10.0f;
        const float MINIMUM = 1.0f;

        mResilience = resilience;

        if (mResilience < MINIMUM)
        {
            mResilience = MINIMUM;
        }
        else if (mResilience > MAXIMUM)
        {
            mResilience = MAXIMUM;
        }
    }

    public void SetSanity(float sanity)
    {
        const float MAXIMUM = 10.0f;
        const float MINIMUM = 1.0f;

        mSanity = sanity;

        if (mSanity < MINIMUM)
        {
            mSanity = MINIMUM;
        }
        else if (mSanity > MAXIMUM)
        {
            mSanity = MAXIMUM;
        }
    }

    public void SetComfortUnderPressure(float comfortUnderPressure)
    {
        const float MAXIMUM = 10.0f;
        const float MINIMUM = 1.0f;

        mComfortUnderPressure = comfortUnderPressure;

        if (mComfortUnderPressure < MINIMUM)
        {
            mComfortUnderPressure = MINIMUM;
        }
        else if (mComfortUnderPressure > MAXIMUM)
        {
            mComfortUnderPressure = MAXIMUM;
        }
    }

    public void SetMaxDeaths(int maxDeaths)
    {
        const int MAXIMUM = 30;
        const int MINIMUM = 10;

        mMaxNumberOfDeaths = maxDeaths;

        if (mMaxNumberOfDeaths < MINIMUM)
        {
            mMaxNumberOfDeaths = MINIMUM;
        }
        else if (mMaxNumberOfDeaths > MAXIMUM)
        {
            mMaxNumberOfDeaths = MAXIMUM;
        }
    }

    // Sets how afraid the character is of a certain fear
    public void SetFearValue(FearType fear, float fearValue)
    {
        const float MAXIMUM = 5.0f;
        const float MINIMUM = -5.0f;

        mFearList[(int) fear] = fearValue;

        if (mFearList[(int)fear] < MINIMUM)
        {
            mFearList[(int)fear] = MINIMUM;
        }
        else if (mFearList[(int)fear] > MAXIMUM)
        {
            mFearList[(int)fear] = MAXIMUM;
        }
    }

    public float GetMaxHealth()
    {
        if (mMaxHealth == DEFAULT_DATA_VALUE)
        {
            Debug.Log("WARNING: Attempt to retrieve unset max health data.");
        }

        return mMaxHealth;
    }

    public float GetMaxStamina()
    {
        if (mMaxStamina == DEFAULT_DATA_VALUE)
        {
            Debug.Log("WARNING: Attempt to retrieve unset max stamina data.");
        }

        return mMaxStamina;
    }

    public float GetSpeed()
    {
        if (mSpeed == DEFAULT_DATA_VALUE)
        {
            Debug.Log("WARNING: Attempt to retrieve unset speed data.");
        }

        return mSpeed;
    }

    public float GetStealth()
    {
        if (mStealth == DEFAULT_DATA_VALUE)
        {
            Debug.Log("WARNING: Attempt to retrieve unset stealth data.");
        }

        return mStealth;
    }

    public float GetReslilience()
    {
        if (mResilience == DEFAULT_DATA_VALUE)
        {
            Debug.Log("WARNING: Attempt to retrieve unset resilience data.");
        }

        return mResilience;
    }

    public float GetSanity()
    {
        if (mSanity == DEFAULT_DATA_VALUE)
        {
            Debug.Log("WARNING: Attempt to retrieve unset sanity data.");
        }

        return mSanity;
    }

    public float GetComfortUnderPressure()
    {
        if (mComfortUnderPressure == DEFAULT_DATA_VALUE)
        {
            Debug.Log("WARNING: Attempt to retrieve unset comfort under pressure data.");
        }

        return mComfortUnderPressure;
    }

    public int GetMaxDeaths()
    {
        if (mMaxNumberOfDeaths == (int) DEFAULT_DATA_VALUE)
        {
            Debug.Log("WARNING: Attempt to retrieve unset max number of deaths data.");
        }

        return mMaxNumberOfDeaths;
    }

    public float GetFearValue(FearType fear)
    {
        return mFearList[(int)fear];
    }

    // Calculates the character difficulty based on data
    public CharacterDifficulty GetCharacterDifficulty()
    {
        if (mMaxHealth == DEFAULT_DATA_VALUE)
        {
            return CharacterDifficulty.NONE;
        }

        float points = mMaxHealth + mMaxStamina + mSpeed + mStealth
                     + mResilience + mSanity + mComfortUnderPressure
                     + mMaxNumberOfDeaths;

        if (points < 200)
        {
            return CharacterDifficulty.HARD;
        }
        else if (points < 250)
        {
            return CharacterDifficulty.MEDIUM;
        }
        else
        {
            return CharacterDifficulty.EASY;
        }
    }


    // ----- CHARACTER INFO UTILITY FUNCTIONS ----- //

    public void SetCharacterName(string name)
    {
        mCharacterName = name;
    }

    public void SetDescription(string description)
    {
        mCharacterDescription = description;
    }

    public string GetCharacterName()
    {
        if (mCharacterName == DEFAULT_SRTING_VALUE)
        {
            Debug.Log("WARNING: Attempt to retrieve unset name data.");
        }

        return mCharacterName;
    }

    public string GetDescription()
    {
        if (mCharacterDescription == DEFAULT_SRTING_VALUE)
        {
            Debug.Log("WARNING: Attempt to retrieve unset description data.");
        }

        return mCharacterDescription;
    }
}
