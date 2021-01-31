using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void StartIntro()
    {
        SceneManager.LoadScene("IntroScreen");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void WinScreen()
    {
        SceneManager.LoadScene("Win");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
