using UnityEngine;

public class SC_GameManager : MonoBehaviour
{
    // ----- VARIABLES ----- //

    private SC_CharacterDataManager.CharacterName mCurrentCharacter = SC_CharacterDataManager.CharacterName.LILLIAN;


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
}
