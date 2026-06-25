using System.Collections.Generic;
using UnityEngine;

public class SC_NPC : MonoBehaviour
{
    // ----- VARIABLES ----- //

    [SerializeField] private string mName;
    [SerializeField] private List<string> mCurrentText;
    [SerializeField] private int mCurrentLine;


    // ----- FUNCTIONS ----- //

    private void Start()
    {
        ResetText();
    }

    public void ResetText()
    {
        mCurrentText.Clear();
        mCurrentText.Add("[Silence]");
        mCurrentLine = 0;
    }

    public void SetToFirstLine()
    {
        mCurrentLine = 0;
    }

    public void NextLine()
    {
        mCurrentLine++;

        if (mCurrentLine == mCurrentText.Count)
        {
            mCurrentLine = mCurrentText.Count - 1;
        }
    }

    public void PrevLine()
    {
        mCurrentLine--;

        if (mCurrentLine < 0)
        {
            mCurrentLine = 0;
        }
    }

    public void SetText(List<string> newText)
    {
        mCurrentText.Clear();
        mCurrentText.AddRange(newText);
    }

    public int GetCurrentLine()
    {
        return mCurrentLine;
    }

    public string GetCurrentLineText()
    {
        return mCurrentText[mCurrentLine];
    }

    public int GetNumLines()
    {
        return mCurrentText.Count;
    }

    public string GetName()
    {
        return mName;
    }
}
