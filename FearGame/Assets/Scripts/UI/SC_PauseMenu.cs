using UnityEngine;

public class SC_PauseMenu : MonoBehaviour
{
    // ----- VARIABLES ----- //

    private SC_GameManager mGameManager;

    [SerializeField] private Canvas mPauseMenuCanvas;


    // ----- FUNCTIONS ----- //

    private void Start()
    {
        mGameManager = FindAnyObjectByType<SC_GameManager>();
    }

    private void Update()
    {
        if (mGameManager.GetGameState() == SC_GameManager.GameState.PAUSED)
        {
            mPauseMenuCanvas.enabled = true;
        }
        else
        {
            mPauseMenuCanvas.enabled = false;
        }
    }
}
