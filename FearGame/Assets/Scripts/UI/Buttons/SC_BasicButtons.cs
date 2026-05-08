using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_BasicButtons : MonoBehaviour
{
    // ----- FUNCTIONS ----- //

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
