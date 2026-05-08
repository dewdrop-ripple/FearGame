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
        LOOTING
    }

    private SC_CharacterDataManager.CharacterName mCurrentCharacter = SC_CharacterDataManager.CharacterName.LILLIAN;

    private GameState mCurrentGameState = GameState.MENU;


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
}
