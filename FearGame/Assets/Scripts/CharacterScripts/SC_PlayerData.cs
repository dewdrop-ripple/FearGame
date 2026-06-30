using System;
using UnityEngine;
using static SC_CharacterData;
using static SC_FearData;

public class SC_PlayerData : MonoBehaviour
{
    // ----- VARIABLES ----- //

    // Game manager for character data
    private SC_GameManager mGameManager;
    private SC_CharacterDataManager mCharacterManager;
    [SerializeField] SC_PlayerMovement mPlayerMovement;

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
    [SerializeField] private float mHunger;

    [SerializeField] private int mNumberOfDeaths;

    // Inventory
    [SerializeField] private SC_CharacterInventory mInventory;
    [SerializeField] private GameObject mCorpsePrefab;
    [SerializeField] private GameObject mDropSpot;

    // Damage
    private const string M_DAMAGE_TAG = "Damage";


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
        
        if (transform.position.y < -100f)
        {
            mHealth = 0;
        }

        if (mHealth <= 0 && mGameManager.GetGameState() != SC_GameManager.GameState.DEAD)
        {
            Die();
        }
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

        mHunger = 0;

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
        if (!mGameManager.AreStatsLocked())
        {
            float adrenalineFactor = ((100.0f - mAdrenaline) / 200.0f) + 0.5f;

            mActualSpeed = (mSpeed - (((mSpeed - 1) / mMaxNumberOfDeaths) * mNumberOfDeaths)) + ((10 - mSpeed) / 100 * mAdrenaline);
            mActualStealth = (mStealth - (((mStealth - 1) / mMaxNumberOfDeaths) * mNumberOfDeaths)) * adrenalineFactor;
            mActualResilience = (mResilience - (((mResilience - 1) / mMaxNumberOfDeaths) * mNumberOfDeaths)) * adrenalineFactor;

            mHunger = mHunger + (((mCharacterManager.GetBaseHungerDrain() / 60) * Time.deltaTime) / adrenalineFactor);
            if (mHunger >= 100)
            {
                mHunger = 100;
                mHealth = mHealth + ((mCharacterManager.GetHungerHealthDrain() / 60) * Time.deltaTime);
            }
            else if (mHunger < mCharacterManager.GetHungerAdrenalineThreshhold())
            {
                mAdrenaline = mAdrenaline + ((mCharacterManager.GetHungerAdrenalineDrain() / 60) * Time.deltaTime);
            }
        }

        mStamina = mStamina + ((mCharacterManager.GetBaseSprintStaminaDrain() / 1.5f) * Time.deltaTime);
        if (mStamina >= mActualMaxStamina) 
        { 
            mStamina = mActualMaxStamina; 
        }
        else if (mStamina < 0) 
        { 
            mStamina = 0; 
        }
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

    public float GetHunger()
    {
        return mHunger;
    }

    public void SetHealth(float health)
    {
        float oldHealth = mHealth;

        mHealth = health;

        if (oldHealth < health)
        {
            mAdrenaline += mCharacterManager.GetHealAdrenalineChange();
        }

        if (mHealth < 0)
        {
            mHealth = 0;
        }
        else if (mHealth > mMaxHealth)
        {
            mHealth = mMaxHealth;
        }
    }

    public void SetStamina(float stamina)
    {
        mStamina = stamina;

        if (mStamina < 0)
        {
            mStamina = 0;
        }
        else if (mStamina > mMaxStamina)
        {
            mStamina = mMaxStamina;
        }
    }

    public void SetAdrenaline(float adrenaline)
    {
        mAdrenaline = adrenaline;

        if (mAdrenaline < 0)
        {
            mAdrenaline = 0;
        }
        else if (mAdrenaline > 100)
        {
            mAdrenaline = 100;
        }
    }

    public void SetNumDeaths(int deaths)
    {
        mNumberOfDeaths = deaths;
    }

    public void SetHunger(float hunger)
    {
        float oldHunger = mHunger;

        mHunger = hunger;

        if (oldHunger > hunger)
        {
            mAdrenaline += mCharacterManager.GetEatAdrenalineChange();
        }

        if (mHunger < 0)
        {
            mHunger = 0;
        }
        else if (mHunger > 100)
        {
            mHunger = 100;
        }
    }


    // ----- INVENTORY FUNCTIONS ----- //

    public bool AddItem(SC_Collectable item)
    {
        return mInventory.AddItem(item);
    }

    public void RemoveItem(SC_Collectable item)
    {
        mInventory.RemoveItem(item);
    }

    public void RemoveItem(int index)
    {
        mInventory.RemoveItem(index);
    }

    public void RemoveItemWithoutDestroying(SC_Collectable item)
    {
        mInventory.RemoveItemWithoutDestroying(item);
    }

    public void RemoveItemWithoutDestroying(int index)
    {
        mInventory.RemoveItem(index);
    }

    public SC_Collectable GetItem(int index)
    {
        return mInventory.GetItem(index);
    }

    public void UseItem(SC_Collectable item)
    {
        mInventory.UseItem(item);
    }

    public void UseItem(int index)
    {
        mInventory.UseItem(index);
    }

    public int GetNumItems()
    {
        return mInventory.GetNumItems();
    }

    public bool Contains(SC_Collectable.CollectableType itemType)
    {
        return mInventory.Contains(itemType);
    }


    // ----- Death and Damage ----- //

    public void Die()
    {
        mPlayerMovement.SetPaused(true);
        mGameManager.SetGameState(SC_GameManager.GameState.DEAD);

        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y - 0.35f, transform.position.z);
        Quaternion spawnRot = transform.rotation;
        GameObject corpse = Instantiate(mCorpsePrefab, spawnPos, spawnRot);

        for (int i = GetNumItems() - 1; i >= 0; i--)
        {
            corpse.GetComponent<SC_Corpse>().AddItem(GetItem(i));
            RemoveItemWithoutDestroying(GetItem(i));
        }
    }

    public void TakeDamage(float damage)
    {
        mHealth -= (damage / 2) + ((damage / 2) / mActualResilience);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == M_DAMAGE_TAG)
        {
            SC_Damage damageObject = other.gameObject.GetComponent<SC_Damage>();
            
            if (damageObject.CanDealDamage())
            {
                damageObject.StartCooldown();
                TakeDamage(damageObject.GetDamage());
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == M_DAMAGE_TAG)
        {
            SC_Damage damageObject = other.gameObject.GetComponent<SC_Damage>();

            if (damageObject.CanDealDamage())
            {
                damageObject.StartCooldown();
                TakeDamage(damageObject.GetDamage());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == M_DAMAGE_TAG)
        {
            SC_Damage damageObject = other.gameObject.GetComponent<SC_Damage>();

            damageObject.CancelCooldown();
        }
    }
}
