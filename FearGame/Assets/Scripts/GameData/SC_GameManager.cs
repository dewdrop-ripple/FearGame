using UnityEngine;

public class SC_GameManager : MonoBehaviour
{
    // ----- VARIABLES ----- //

    public enum GameState
    {
        MENU,
        INVENTORY,
        PAUSED,
        PLAYING,
        LOOTING,
        DEAD,
        TALKING_TO_NPC
    }

    private SC_CharacterDataManager.CharacterName mCurrentCharacter = SC_CharacterDataManager.CharacterName.LILLIAN;

    private GameState mCurrentGameState = GameState.MENU;

    [SerializeField] private GameObject mPlayerPrefab;

    [SerializeField] private bool mLockPlayerStats = false;


    // ----- FUNCTIONS ----- //

    // Singleton
    private void Awake()
    {
        if (FindObjectsByType<SC_GameManager>().Length > 1)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (!FindAnyObjectByType<SC_PlayerData>() && FindAnyObjectByType<SC_Spawn>())
        {
            mCurrentGameState = GameState.PLAYING;
            Vector3 spawnPos = FindAnyObjectByType<SC_Spawn>().gameObject.transform.position;
            Quaternion spawnRot = FindAnyObjectByType<SC_Spawn>().gameObject.transform.rotation;
            GameObject corpse = Instantiate(mPlayerPrefab, spawnPos, spawnRot);
        }
    }


    // ----- UITILITY FUNCTIONS ----- //

    public void SetCurrentCharacter(SC_CharacterDataManager.CharacterName character)
    {
        mCurrentCharacter = character;
    }

    public SC_CharacterDataManager.CharacterName GetCurrentCharacter()
    {
        return mCurrentCharacter;
    }

    public GameState GetGameState()
    {
        return mCurrentGameState;
    }

    public void SetGameState(GameState newState)
    {
        mCurrentGameState = newState;
    }

    public bool AreStatsLocked()
    {
        return mLockPlayerStats;
    }

    public void SetPlayerStatsLocked(bool isLocked)
    {
        mLockPlayerStats = isLocked;
    }
}
