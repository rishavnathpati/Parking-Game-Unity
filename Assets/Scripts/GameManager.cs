using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject welcomePanel;

    private void Start()
    {
        if (PlayerPrefs.GetInt("Rules") == 1)
            welcomePanel.SetActive(false);
    }

    public void GameOverYes()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameOverQuit()
    {
        Application.Quit();
    }

    public void GameWinContinue()
    {
        if (SceneManager.GetActiveScene().buildIndex < 3)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void GameWinRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RulesNext()
    {
        PlayerPrefs.SetInt("Rules", 1);
    }
}