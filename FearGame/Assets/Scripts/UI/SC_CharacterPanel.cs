using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SC_CharacterPanel : MonoBehaviour
{
    // ----- VARIABLES ----- //

    // UI elements to resize
    [SerializeField] private float mPanelGaps;
    [SerializeField] private float mPanelOneScreenPercent;
    private float mPanelTwoScreenPercent;
    [SerializeField] private List<Button> mCharacterButtons;
    [SerializeField] private GameObject mPanelOne;
    [SerializeField] private GameObject mPanelTwo;

    // UI elements to edit data in
    [SerializeField] private TextMeshProUGUI mTitle;
    [SerializeField] private TextMeshProUGUI mDescription;
    [SerializeField] private List<Slider> mDataSliders;
    [SerializeField] private TextMeshProUGUI mStartText;

    // Character data
    private SC_CharacterDataManager mCharacterData;
    SC_CharacterDataManager.CharacterName mTargetCharacter = SC_CharacterDataManager.CharacterName.LILLIAN;


    // ----- FUNCTIONS ----- //

    // Get character data
    private void Start()
    {
        mCharacterData = GameObject.FindAnyObjectByType<SC_CharacterDataManager>();
        UpdateData();
    }

    // Update the sizes of the panels based on the screen size
    private void Update()
    {
        
    }

    // Switch target character
    public void SetTargetChatacter(SC_CharacterDataManager.CharacterName name)
    {
        mTargetCharacter = name;
        UpdateData();
    }

    public SC_CharacterDataManager.CharacterName GetTargetCharacter()
    {
        return mTargetCharacter;
    }

    // Update the data in the character data panel
    private void UpdateData()
    {
        mTitle.SetText(mCharacterData.GetCharacterData(mTargetCharacter).GetCharacterName());
        mDescription.SetText(mCharacterData.GetCharacterData(mTargetCharacter).GetDescription());

        mDataSliders[0].value = mCharacterData.GetCharacterData(mTargetCharacter).GetMaxHealth();
        mDataSliders[1].value = mCharacterData.GetCharacterData(mTargetCharacter).GetMaxStamina();
        mDataSliders[2].value = mCharacterData.GetCharacterData(mTargetCharacter).GetSpeed();
        mDataSliders[3].value = mCharacterData.GetCharacterData(mTargetCharacter).GetStealth();
        mDataSliders[4].value = mCharacterData.GetCharacterData(mTargetCharacter).GetReslilience();
        mDataSliders[5].value = mCharacterData.GetCharacterData(mTargetCharacter).GetSanity();
        mDataSliders[6].value = mCharacterData.GetCharacterData(mTargetCharacter).GetComfortUnderPressure();
        mDataSliders[7].value = mCharacterData.GetCharacterData(mTargetCharacter).GetMaxDeaths();

        mStartText.SetText("Play as " + mCharacterData.GetCharacterData(mTargetCharacter).GetCharacterName());
    }
}
