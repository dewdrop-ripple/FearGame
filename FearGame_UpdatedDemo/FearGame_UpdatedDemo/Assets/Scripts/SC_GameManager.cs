using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_GameManager : MonoBehaviour
{
    // --- Basic Data --- //
    public enum GameState
    {
        PLAYING,
        PAUSED,
        INVENTORY,
        LOOTING,
        TALKING,
        DEAD
    }

    [SerializeField] private GameState state;
    [SerializeField] private bool newState;

    [SerializeField] private float playerSpawnDelay;
    private float time;

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
                case GameState.PAUSED:
                case GameState.DEAD:
                    if (openStorageUnit != null) openStorageUnit.CloseMenu();
                    if (targetNPC != null) targetNPC.Close();
                    if (inventory != null) inventory.CloseMenu();
                    break;

                case GameState.INVENTORY:
                    if (openStorageUnit != null) openStorageUnit.CloseMenu();
                    if (targetNPC != null) targetNPC.Close();
                    if (inventory != null) inventory.OpenMenu();
                    break;

                case GameState.LOOTING:
                    if (openStorageUnit != null) openStorageUnit.OpenMenu();
                    if (targetNPC != null) targetNPC.Close();
                    if (inventory != null) inventory.OpenMenu();
                    break;

                case GameState.TALKING:
                    if (openStorageUnit != null) openStorageUnit.CloseMenu();
                    if (targetNPC != null) targetNPC.Open();
                    if (inventory != null) inventory.CloseMenu();
                    break;
            }

            newState = false;
        }

        switch (state)
        {
            case GameState.PLAYING:
            case GameState.DEAD:
                backgroundCanavs.GetComponent<Canvas>().enabled = false;
                break;

            case GameState.PAUSED:
            case GameState.INVENTORY:
            case GameState.LOOTING:
            case GameState.TALKING:
                backgroundCanavs.GetComponent<Canvas>().enabled = true;
                break;
        }

        if (GameObject.FindGameObjectsWithTag("Player").Length < 1 && GameObject.FindGameObjectsWithTag("PlayerSpawn").Length >= 1)
        {
            GameObject spawnedPlayer = Instantiate(player);
            spawnedPlayer.transform.position = GameObject.FindGameObjectWithTag("PlayerSpawn").transform.position;
            spawnedPlayer.transform.rotation = GameObject.FindGameObjectWithTag("PlayerSpawn").transform.rotation;
            time = 0.0f;
        }

        if (SceneManager.GetActiveScene().name == "MP_StartMenu")
        {
            UnlockCursor();
        }

        if (time < playerSpawnDelay)
        {
            time += Time.deltaTime;
        }
        else if (time < 100000)
        {
            if(GameObject.FindGameObjectWithTag("Player")) GameObject.FindGameObjectWithTag("Player").GetComponent<SC_Player>().Enable();
            time = 100001;
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


    // --- Player Spawn --- //

    [SerializeField] GameObject player;


    // --- NPCs --- //

    [SerializeField] private SC_NPC targetNPC = null;

    public void SetTargetNPC(SC_NPC npc)
    {
        targetNPC = npc;
    }

    public void RemoveTargetNPC()
    {
        targetNPC = null;
    }

    public SC_NPC GetTargetNPC()
    {
        return targetNPC;
    }


    // --- Cursor --- //

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    // --- Scene Changing --- //

    public void SceneChanged()
    {
        SetGameState(GameState.PLAYING);
        LockCursor();
    }
}
