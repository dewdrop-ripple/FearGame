using UnityEngine;

public class SC_ItemManager : MonoBehaviour
{
    public enum ItemState
    {
        STORED,
        DROPPED
    }

    public enum EffectType
    {
        NONE,
        HEALTH,
        HUNGER,
        ADRENALINE
    }


    // --- GENERAL DATA --- //

    [SerializeField] private ItemState state;

    [SerializeField] private EffectType effectType;
    [SerializeField] private float effectStrength;

    [SerializeField] private string itemName;
    [SerializeField] private string itemDescription;

    private SC_GameManager gameManager;


    private void Start()
    {
        gameManager = FindAnyObjectByType<SC_GameManager>();

        icon = GetComponent<UnityEngine.UI.Image>();

        StartUI();
    }

    private void Update()
    {
        switch (state)
        {
            case ItemState.STORED:
                UIUpdate();
                break;

            case ItemState.DROPPED:
                ObjectUpdate();
                break;
        }
    }

    public string GetName()
    {
        return itemName;
    }

    public string GetDescription()
    {
        return itemDescription;
    }


    // --- UI SYSTEMS --- //

    private Vector3 oldUIPosition;
    private Vector3 targetUIPosition;

    private Vector2 UISelectOffset;

    [SerializeField] private float iconCollisionRadius;

    private bool isClicked = false;

    private UnityEngine.UI.Image icon;
    private Color baseColor;
    private Color selectedColor;

    private bool showInfo = false;
    [SerializeField] private GameObject infoPanel;

    [SerializeField] private SC_StorageManager storageManager;
    [SerializeField] private int storedSlot;


    private void StartUI()
    {
        storageManager.AddItemToSlot(storedSlot, this);

        targetUIPosition = storageManager.GetSlotPos(storedSlot);
        oldUIPosition = targetUIPosition;

        baseColor = icon.color;
        selectedColor = new Color(baseColor.r / 2.0f, baseColor.g / 2.0f, baseColor.b / 2.0f);

        iconCollisionRadius = GetComponent<RectTransform>().rect.width;
        iconCollisionRadius *= Screen.height / 1250.0f;
    }

    private void UIUpdate()
    {
        // Controls
        if (Input.GetMouseButtonDown(0)) // Left Click
        {
            HideInfo();

            Vector2 mouseScreenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 position2D = new Vector2(oldUIPosition.x, oldUIPosition.y);

            if (Vector2.Distance(mouseScreenPosition, position2D) < iconCollisionRadius)
            {
                UISelectOffset = new Vector2(oldUIPosition.x - Input.mousePosition.x, oldUIPosition.y - Input.mousePosition.y);

                isClicked = true;
            }
        }
        else if (Input.GetMouseButtonUp(0)) // Left Released
        {
            if (isClicked)
            {
                PickNewSlot();
                isClicked = false;
            }
        }

        else if (Input.GetMouseButtonDown(1)) // Right Click
        {
            Vector2 mouseScreenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 position2D = new Vector2(oldUIPosition.x, oldUIPosition.y);

            if (Vector2.Distance(mouseScreenPosition, position2D) < iconCollisionRadius)
            {
                showInfo = true;
                
                if(infoPanel.GetComponent<SC_ItemInfoPanel>() != null)
                {
                    infoPanel.GetComponent<SC_ItemInfoPanel>().OpenMenu(this);
                }
            }
            else
            {
                HideInfo();
            }
        }


        // Getting Dragged
        if (isClicked)
        {
            icon.color = selectedColor;

            Vector2 mouseScreenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            targetUIPosition = mouseScreenPosition + UISelectOffset;

            transform.SetParent(storageManager.GetActiveParent().transform, true);
        }
        else
        {
            icon.color = baseColor;

            targetUIPosition = oldUIPosition;

            transform.SetParent(storageManager.GetInactiveParent().transform, true);
        }

        transform.position = targetUIPosition;
    }

    private void HideInfo()
    {
        if (showInfo)
        {
            showInfo = false;

            if (infoPanel.GetComponent<SC_ItemInfoPanel>() != null)
            {
                infoPanel.GetComponent<SC_ItemInfoPanel>().CloseMenu();
            }
        }
    }

    private void PickNewSlot()
    {
        if (storageManager.GetStorageType() == SC_StorageManager.StorageType.INVENTORY)
        {
            if (transform.position.x < Screen.width / 2.0f) // Still Here
            {
                CheckCurrentStorageUnit();
            }
            else
            {
                CheckNewStorageUnit(gameManager.GetOpenStorageUnit());
            }
        }
        else
        {
            if (transform.position.x > Screen.width / 2.0f) // Still Here
            {
                CheckCurrentStorageUnit();
            }
            else
            {
                CheckNewStorageUnit(gameManager.GetInventory());
            }
        }
    }

    public void SetPosition(Vector3 position)
    {
        targetUIPosition = position;
        oldUIPosition = position;
        isClicked = false;
    }

    public void SetSlot(int slot)
    {
        storedSlot = slot;
    }

    public void SetUnit(SC_StorageManager newStorageManager)
    {
        storageManager = newStorageManager;
    }

    public void CheckCurrentStorageUnit()
    {
        int nearestSlot = storageManager.GetNearestSlot(targetUIPosition);

        if (nearestSlot == -1)
        {
            targetUIPosition = oldUIPosition;
        }
        else if (!storageManager.SlotIsOpen(nearestSlot))
        {
            storageManager.SwapSlots(nearestSlot, storedSlot);
        }
        else
        {
            SetPosition(storageManager.GetSlotPos(nearestSlot));
            storageManager.RemoveItemFromSlot(storedSlot);
            storedSlot = nearestSlot;
            storageManager.AddItemToSlot(storedSlot, this);
        }
    }

    public void CheckNewStorageUnit(SC_StorageManager newStorageUnit)
    {
        int nearestSlot = newStorageUnit.GetNearestSlot(targetUIPosition);

        if (nearestSlot == -1)
        {
            targetUIPosition = oldUIPosition;
        }
        else if (!newStorageUnit.SlotIsOpen(nearestSlot))
        {
            storageManager.SwapAcrossUnits(newStorageUnit, storedSlot, nearestSlot);
        }
        else
        {
            SetPosition(newStorageUnit.GetSlotPos(nearestSlot));
            storageManager.RemoveItemFromSlot(storedSlot);
            storedSlot = nearestSlot;
            newStorageUnit.AddItemToSlot(storedSlot, this);

            storageManager = newStorageUnit;
        }
    }

    public void SetStorageManager(SC_StorageManager manager)
    {
        storageManager = manager;
    }


    // --- OBJECT SYSTEMS --- //

    private void ObjectUpdate()
    {

    }
}
