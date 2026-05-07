using UnityEngine;

public class SC_Inventory : MonoBehaviour
{
    // ----- VARIABLES ----- //

    private SC_GameManager mGameManager;

    [SerializeField] private Canvas mInventoryCanvas;


    // ----- FUNCTIONS ----- //

    private void Start()
    {
        mGameManager = FindAnyObjectByType<SC_GameManager>();
    }

    private void Update()
    {
        if (mGameManager.GetGameState() == SC_GameManager.GameState.INVENTORY)
        {
            mInventoryCanvas.enabled = true;
        }
        else
        {
            mInventoryCanvas.enabled = false;
        }
    }
}
