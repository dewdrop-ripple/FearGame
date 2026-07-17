using UnityEngine;

public class SC_StorageObject : MonoBehaviour
{
    private SC_GameManager gameManager;

    [SerializeField] private SC_StorageUnit storageUnit;

    private void Start()
    {
        gameManager = FindAnyObjectByType<SC_GameManager>();
    }

    public void Open()
    {
        storageUnit.OpenMenu();
        gameManager.SetOpenStorageUnit(storageUnit);
        gameManager.SetGameState(SC_GameManager.GameState.LOOTING);
    }

    public void Close()
    {
        storageUnit.CloseMenu();
        gameManager.CloseStorageUnit();
        gameManager.SetGameState(SC_GameManager.GameState.PLAYING);
    }

    public SC_StorageUnit GetStorageUnit()
    {
        return storageUnit;
    }
}
