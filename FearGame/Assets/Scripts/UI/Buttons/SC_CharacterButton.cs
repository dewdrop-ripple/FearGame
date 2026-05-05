using UnityEngine;

public class SC_CharacterButton : MonoBehaviour
{
    // ----- VARIABLES ----- //

    [SerializeField] SC_CharacterDataManager.CharacterName mCharacterName;
    [SerializeField] SC_CharacterPanel mParentPanel;


    // ----- FUNCTIONS ----- //

    // Update the data according to the button clicked
    public void ButtonClicked()
    {
        mParentPanel.SetTargetChatacter(mCharacterName);
    }
}
