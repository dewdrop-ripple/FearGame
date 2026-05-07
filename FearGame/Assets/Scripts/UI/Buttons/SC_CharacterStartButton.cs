using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_CharacterStartButton : MonoBehaviour
{
    // ----- VARIABLES ----- //

    [SerializeField] private string mNextSceneName;
    [SerializeField] private SC_CharacterPanel mCharacterPanel;
    private SC_GameManager mGameManager;


    // ----- FUNCTIONS ----- //

    private void Start()
    {
        mGameManager = FindAnyObjectByType<SC_GameManager>();
    }

    public void ButtonClicked()
    {
        mGameManager.SetCurrentCharacter(mCharacterPanel.GetTargetCharacter());
        mGameManager.SetGameState(SC_GameManager.GameState.PLAYING);
        SceneManager.LoadScene(mNextSceneName);
    }
}
