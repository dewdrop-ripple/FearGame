using UnityEngine;

public class SC_DeathMenu : MonoBehaviour
{
    // ----- VARIABLES ----- //

    private SC_GameManager mGameManager;

    [SerializeField] private Canvas mPauseMenuCanvas;
    [SerializeField] private GameObject mPlayer;


    // ----- FUNCTIONS ----- //

    private void Start()
    {
        mGameManager = FindAnyObjectByType<SC_GameManager>();
    }

    private void Update()
    {
        if (mGameManager.GetGameState() == SC_GameManager.GameState.DEAD)
        {
            mPauseMenuCanvas.enabled = true;
        }
        else
        {
            mPauseMenuCanvas.enabled = false;
        }
    }


    // ----- BUTTONS ----- //

    public void KillPlayer()
    {
        Destroy(mPlayer);
    }
}
