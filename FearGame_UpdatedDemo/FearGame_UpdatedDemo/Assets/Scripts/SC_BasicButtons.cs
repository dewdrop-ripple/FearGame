using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_BasicButtons : MonoBehaviour
{
    // --- General --- //

    private SC_GameManager gameManager;

    private void Start()
    {
        gameManager = FindAnyObjectByType<SC_GameManager>();
    }


    // --- Buttons --- //

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void DebugLogMessage(string message)
    {
        Debug.Log(message);
    }

    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        gameManager.SceneChanged();
    }
}
