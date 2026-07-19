using TMPro;
using UnityEngine;

public class SC_ItemInfoPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;

    [SerializeField] private bool isVisible = false;

    [SerializeField] private SC_Item targetItem;

    [SerializeField] private Vector3 targetPosition;
    private Vector3 hiddenPosition;

    [SerializeField] private float disappearDelay;
    [SerializeField] private float time = -1.0f;


    private void Awake()
    {
        hiddenPosition = new Vector3(Screen.width * 2.0f, 0.0f, 0.0f);
        targetPosition = hiddenPosition;
    }

    private void Update()
    {
        if (targetItem == null)
        {
            isVisible = false;
            targetPosition = hiddenPosition;
            transform.position = targetPosition;
        }
        else
        {
            itemName.SetText(targetItem.GetName());
            itemDescription.SetText(targetItem.GetDescription());
        }

        if (isVisible)
        {
            Vector2 offset = new Vector2(GetComponent<RectTransform>().rect.width * Screen.width / 2500.0f * 1.45f, GetComponent<RectTransform>().rect.height * Screen.height / 1250.0f * 1.35f);
            targetPosition = new Vector3(targetItem.transform.position.x + offset.x, targetItem.transform.position.y - offset.y, targetItem.transform.position.z);

            float screenEdgeBuffer = Screen.width * 0.075f;

            if (targetPosition.x + GetComponent<RectTransform>().rect.width + screenEdgeBuffer >= Screen.width)
            {
                targetPosition.x = Screen.width - screenEdgeBuffer - GetComponent<RectTransform>().rect.width;
            }
            else if (targetPosition.x - screenEdgeBuffer <= 0)
            {
                targetPosition.x = screenEdgeBuffer;
            }

            if (targetPosition.y + GetComponent<RectTransform>().rect.height + screenEdgeBuffer >= Screen.height)
            {
                targetPosition.y = Screen.height - screenEdgeBuffer - GetComponent<RectTransform>().rect.height;
            }
            else if (targetPosition.y - screenEdgeBuffer <= 0)
            {
                targetPosition.y = screenEdgeBuffer;
            }
        }
        else
        {
            targetPosition = hiddenPosition;
        }

        if (time >= 0.0f)
        {
            if (time < disappearDelay)
            {
                time += Time.deltaTime;
            }
            else
            {
                time = -1.0f;

                if (isVisible)
                {
                    isVisible = false;
                }
                else
                {
                    isVisible = true;
                }
            }
        }
        else
        {
            transform.position = targetPosition;
        }
    }

    public void OpenMenu(SC_Item target)
    {
        isVisible = false;

        targetItem = target;
        time = 0.0f;
    }

    public void CloseMenu()
    {
        time = 0.0f;
    }

    public void DebugLogMessage(string message)
    {
        Debug.Log(message);
    }

    public void ClearTargetItem()
    {
        targetItem = null;
    }

    public void DropTargetItem()
    {
        targetItem.Drop(FindAnyObjectByType<SC_Player>().transform.position);
    }

    public void UseTargetItem()
    {
        targetItem.Use(FindAnyObjectByType<SC_Player>());
    }
}
