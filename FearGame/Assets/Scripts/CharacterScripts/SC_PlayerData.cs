using Unity.VisualScripting;
using UnityEditor.Overlays;
using UnityEngine;
using static SC_CharacterData;
using static SC_FearData;

public class SC_PlayerData : MonoBehaviour
{
    // ----- VARIABLES ----- //

    // Game manager for character data
    private SC_GameManager mGameManager;
    private SC_CharacterDataManager mCharacterManager;

    // Base character data
    private SC_CharacterDataManager.CharacterName mCurrentCharacter;

    [SerializeField] private float mMaxHealth;
    [SerializeField] private float mMaxStamina;
    [SerializeField] private float mSpeed;
    [SerializeField] private float mStealth;
    [SerializeField] private float mResilience;
    [SerializeField] private float mSanity;
    [SerializeField] private float mComfortUnderPressure;
    [SerializeField] private int mMaxNumberOfDeaths;

    [SerializeField] private float[] mFearList;

    // Other player stats
    [SerializeField] private float mActualMaxHealth;
    [SerializeField] private float mActualMaxStamina;
    [SerializeField] private float mActualSpeed;
    [SerializeField] private float mActualStealth;
    [SerializeField] private float mActualResilience;

    [SerializeField] private float mAdrenaline;
    [SerializeField] private float mHealth;
    [SerializeField] private float mStamina;
    [SerializeField] private float mHungerLeft;

    [SerializeField] int mNumberOfDeaths;


    // ----- FUNCTIONS ----- //

    private void Start()
    {
        mGameManager = FindAnyObjectByType<SC_GameManager>();
        mCharacterManager = FindAnyObjectByType<SC_CharacterDataManager>();

        ResetStats();
    }

    private void Update()
    {
        UpdateStats();
    }

    // Reset player stats to the base starting stats
    private void ResetStats()
    {
        mCurrentCharacter = mGameManager.GetCurrentCharacter();

        SC_CharacterData baseData = mCharacterManager.GetCharacterData(mCurrentCharacter);

        mMaxHealth = baseData.GetMaxHealth();
        mMaxStamina = baseData.GetMaxStamina();

        mActualMaxHealth = mMaxHealth;
        mActualMaxStamina = mMaxStamina;

        mSpeed = baseData.GetSpeed();
        mStealth = baseData.GetStealth();
        mResilience = baseData.GetReslilience();
        mSanity = baseData.GetComfortUnderPressure();
        mComfortUnderPressure = baseData.GetSanity();
        mMaxNumberOfDeaths = baseData.GetMaxDeaths();

        mHungerLeft = 100;

        mFearList = new float[(int) SC_FearData.FearType.NUMBER_OF_FEARS];

        for (int i = 0; i < mFearList.Length; i++)
        {
            mFearList[i] = baseData.GetFearValue((SC_FearData.FearType)i);
        }

        mAdrenaline = mCharacterManager.GetBaseAdrenaline();
        mHealth = mMaxHealth;
        mStamina = mMaxStamina;
        mNumberOfDeaths = 0;
    }

    // Update stats based on adrenaline and number of deaths
    private void UpdateStats()
    {
        float adrenalineFactor = ((100.0f - mAdrenaline) / 200.0f) + 0.5f;

        mActualSpeed = (mSpeed - (((mSpeed - 1) / mMaxNumberOfDeaths) * mNumberOfDeaths)) * adrenalineFactor;
        mActualStealth = (mStealth - (((mStealth - 1) / mMaxNumberOfDeaths) * mNumberOfDeaths)) * adrenalineFactor;
        mActualResilience = (mResilience - (((mResilience - 1) / mMaxNumberOfDeaths) * mNumberOfDeaths)) * adrenalineFactor;
    }


    // ----- UITILITY FUNCTIONS ----- //

    public float GetMaxHealth()
    {
        return mActualMaxHealth;
    }

    public float GetMaxStamina()
    {
        return mActualMaxStamina;
    }

    public float GetSpeed()
    {
        return mActualSpeed;
    }

    public float GetStealth()
    {
        return mActualStealth;
    }

    public float GetReslilience()
    {
        return mActualResilience;
    }

    public float GetSanity()
    {
        return mSanity;
    }

    public float GetComfortUnderPressure()
    {
        return mComfortUnderPressure;
    }

    public int GetMaxDeaths()
    {
        return mMaxNumberOfDeaths;
    }

    public float GetFearValue(FearType fear)
    {
        return mFearList[(int) fear];
    }

    public CharacterDifficulty GetCharacterDifficulty()
    {
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

    public float GetHealth()
    {
        return mHealth;
    }

    public float GetStamina()
    {
        return mStamina;
    }

    public float GetAdrenaline()
    {
        return mAdrenaline;
    }

    public int GetNumDeaths()
    {
        return mNumberOfDeaths;
    }

    public float GetHungerLeft()
    {
        return mHungerLeft;
    }

    public void SetHealth(float health)
    {
        mHealth = health;
    }

    public void SetStamina(float stamina)
    {
        mStamina = stamina;
    }

    public void SetAdrenaline(float adrenaline)
    {
        mAdrenaline = adrenaline;
    }

    public void SetNumDeaths(int deaths)
    {
        mNumberOfDeaths = deaths;
    }

    public void SetHungerLeft(float hungerLeft)
    {
        mHungerLeft = hungerLeft;
    }
}
