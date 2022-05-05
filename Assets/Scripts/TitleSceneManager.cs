using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void BackToTitle()
    {
        SceneManager.LoadScene(0);
    }

    public void HowToPlayScreen()
    {
        SceneManager.LoadScene(2);
    }

    public void ControlsScreen()
    {
        SceneManager.LoadScene(3);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
