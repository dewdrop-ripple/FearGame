using UnityEngine;

public class SC_GameManager : MonoBehaviour
{
    // --- Basic Data --- //
    public enum GameState
    {
        PLAYING,
        PAUSED,
        INVENTORY,
        LOOTING
    }

    [SerializeField] private GameState state;
    [SerializeField] private bool newState;


    // Singleton
    private void Awake()
    {
        if (FindObjectsByType<SC_GameManager>().Length > 1)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        state = GameState.PLAYING;
    }

    public GameState GetGameState()
    {
        return state;
    }

    public void SetGameState(GameState state)
    {
        this.state = state;
        newState = true;
    }

    private void Update()
    {
        if (newState)
        {
            switch (state)
            {
                case GameState.PLAYING:
                    if (openStorageUnit != null) openStorageUnit.CloseMenu();
                    inventory.CloseMenu();
                    backgroundCanavs.SetActive(false);
                    break;

                case GameState.PAUSED:
                    if (openStorageUnit != null) openStorageUnit.CloseMenu();
                    inventory.CloseMenu();
                    backgroundCanavs.SetActive(true);
                    break;

                case GameState.INVENTORY:
                    if (openStorageUnit != null) openStorageUnit.CloseMenu();
                    inventory.OpenMenu();
                    backgroundCanavs.SetActive(true);
                    break;

                case GameState.LOOTING:
                    if (openStorageUnit != null) openStorageUnit.OpenMenu();
                    inventory.OpenMenu();
                    backgroundCanavs.SetActive(true);
                    break;
            }

            newState = false;
        }
    }


    // --- Storage --- //

    [SerializeField] private SC_StorageUnit openStorageUnit = null;
    [SerializeField] private SC_StorageUnit inventory;

    [SerializeField] private GameObject backgroundCanavs;
    [SerializeField] private GameObject foregroundCanavs;


    public SC_StorageUnit GetOpenStorageUnit()
    {
        return openStorageUnit;
    }

    public void SetOpenStorageUnit(SC_StorageUnit storageUnit)
    {
        openStorageUnit = storageUnit;
    }

    public void CloseStorageUnit()
    {
        openStorageUnit = null;
    }

    public bool IsStorageOpen()
    {
        return openStorageUnit != null;
    }

    public SC_StorageUnit GetInventory()
    {
        return inventory;
    }

    public void SetInventory(SC_StorageUnit storageUnit)
    {
        inventory = storageUnit;
    }

    public GameObject GetForegroundCanvas()
    {
        return foregroundCanavs;
    }
}
