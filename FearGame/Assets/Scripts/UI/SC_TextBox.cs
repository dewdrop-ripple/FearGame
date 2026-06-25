using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SC_TextBox : MonoBehaviour
{
    // ----- VARIABLES ----- //

    private SC_GameManager mGameManager;

    // Text Data
    [SerializeField] private SC_PlayerData mAttachedPlayer;
    [SerializeField] private SC_PlayerMovement mAttachedPlayerMovement;

    [SerializeField] private SC_NPC mTargetNPC;

    // UI Data
    [SerializeField] private Canvas mTextBoxCanvas;

    [SerializeField] private TextMeshProUGUI mNameText;
    [SerializeField] private TextMeshProUGUI mSpeechText;

    [SerializeField] private Button mNextButton;
    [SerializeField] private Button mPreviousButton;
    [SerializeField] private Button mCloseButton;


    // ----- FUNCTIONS ----- //

    private void Start()
    {
        mGameManager = FindAnyObjectByType<SC_GameManager>();
    }

    private void Update()
    {
        if (mGameManager.GetGameState() == SC_GameManager.GameState.TALKING_TO_NPC)
        {
            mTextBoxCanvas.enabled = true;

            mNameText.SetText(mTargetNPC.GetName());
            mSpeechText.SetText(mTargetNPC.GetCurrentLineText());

            if (mTargetNPC.GetCurrentLine() == 0)
            {
                mPreviousButton.interactable = false;
            }
            else
            {
                mPreviousButton.interactable = true;
            }

            if (mTargetNPC.GetCurrentLine() == mTargetNPC.GetNumLines() - 1)
            {
                mNextButton.interactable = false;
            }
            else
            {
                mNextButton.interactable = true;
            }
        }
        else
        {
            mTextBoxCanvas.enabled = false;
        }
    }

    public void SetNPC(SC_NPC npc)
    {
        Debug.Log("Body Set");
        mTargetNPC = npc;
    }

    public void OpenMenu()
    {
        mAttachedPlayerMovement.SetPaused(true);

        mTargetNPC.SetToFirstLine();

        mGameManager.SetGameState(SC_GameManager.GameState.TALKING_TO_NPC);
    }


    // ----- BUTTONS ----- //

    public void NextLine()
    {
        mTargetNPC.NextLine();
        mSpeechText.SetText(mTargetNPC.GetCurrentLineText());
    }

    public void PrevLine()
    {
        mTargetNPC.PrevLine();
        mSpeechText.SetText(mTargetNPC.GetCurrentLineText());
    }

    public void CloseMenu()
    {
        mAttachedPlayerMovement.SetPaused(false);
        mGameManager.SetGameState(SC_GameManager.GameState.PLAYING);
    }
}
