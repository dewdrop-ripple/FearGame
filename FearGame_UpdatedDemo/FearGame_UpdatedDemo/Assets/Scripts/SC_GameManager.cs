using UnityEngine;

public class SC_GameManager : MonoBehaviour
{
    // --- Basic Data --- //

    // Singleton
    private void Awake()
    {
        if (FindObjectsByType<SC_GameManager>().Length > 1)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }


    // --- Storage --- //

    [SerializeField] private SC_StorageManager openStorageUnit = null;
    [SerializeField] private SC_StorageManager inventory;


    public SC_StorageManager GetOpenStorageUnit()
    {
        return openStorageUnit;
    }

    public void SetOpenStorageUnit(SC_StorageManager storageUnit)
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

    public SC_StorageManager GetInventory()
    {
        return inventory;
    }

    public void SetInventory(SC_StorageManager storageUnit)
    {
        inventory = storageUnit;
    }
}
