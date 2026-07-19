using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SC_NPC : MonoBehaviour
{
    // --- General --- //

    private SC_GameManager gameManager;

    private void Start()
    {
        gameManager = FindAnyObjectByType<SC_GameManager>();

        SetConversation(0);
    }

    public void Open()
    {
        // TEMPORARY
        // vvvvv

        SetConversation((int) Mathf.Floor(Random.Range(0.0f, ((float) numConversations) - 0.1f)));

        // ^^^^^

        gameManager.SetGameState(SC_GameManager.GameState.TALKING);
        gameManager.SetTargetNPC(this);
        textNum = 0;
        isVisible = true;
    }

    public void Close()
    {
        gameManager.SetGameState(SC_GameManager.GameState.PLAYING);
        gameManager.RemoveTargetNPC();
        isVisible = false;
    }


    // --- Text --- //

    private List<string> currentConversation = new List<string>();

    [SerializeField] private List<string> allCoversations;
    [SerializeField] private int numConversations;
    [SerializeField] private List<Vector2> conversationStartAndEnds;
    [SerializeField] private int conversationNum;

    [SerializeField] private int textNum = 0;

    public void Next()
    {
        textNum++;

        if (textNum >= currentConversation.Count)
        {
            textNum = currentConversation.Count - 1;
        }
    }

    public void Previous()
    {
        textNum--;

        if (textNum < 0)
        {
            textNum = 0;
        }
    }

    public void SetConversation(int conversation)
    {
        if (conversation >= 0 && conversation < numConversations)
        {
            textNum = 0;

            int start = (int) conversationStartAndEnds[conversation].x;
            int end = (int)conversationStartAndEnds[conversation].y;

            currentConversation.Clear();

            for (int i = start; i <= end; i++)
            {
                currentConversation.Add(allCoversations[i]);
            }
        }
    }


    // --- Canvas --- //

    bool isVisible = false;

    [SerializeField] private Canvas canvas;
    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private Button nextButton;
    [SerializeField] private Button prevButton;


    private void Update()
    {
        canvas.enabled = isVisible;

        if (isVisible)
        {
            if (currentConversation.Count == 0)
            {
                text.SetText("...");
            }
            else
            {
                text.SetText(currentConversation[textNum]);
            }

            if (textNum == currentConversation.Count - 1)
            {
                nextButton.interactable = false;
                return;
            }
            else
            {
                nextButton.interactable = true;
            }

            if (textNum == 0)
            {
                prevButton.interactable = false;
            }
            else
            {
                prevButton.interactable = true;
            }
        }
    }
}
