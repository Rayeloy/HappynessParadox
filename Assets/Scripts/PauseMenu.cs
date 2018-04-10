using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public static PauseMenu instance;

    public GameObject pauseMenu;
    public Text pauseText;

    private void Awake()
    {
        instance = this;
        pauseText.color = new Color(1, 1, 1, 0.5f);
    }

    private void OnMouseEnter()
    {
        pauseText.color = new Color(1, 1, 1, 0.8f);
    }

    private void OnMouseExit()
    {
        pauseText.color = new Color(1, 1, 1, 0.5f);
    }

    private void OnMouseDrag()
    {
        pauseText.color = new Color(1, 1, 1, 1f);
    }

    private void OnMouseUp()
    {
        pauseText.color = new Color(1, 1, 1, 0.8f);
        Pausar();
    }

    public void Pausar()
    {
        if (GameController.instance.playing)
        {
            Time.timeScale = 0;
            GameController.instance.playing = false;
            GameController.instance.Veil.enabled = true;
            pauseMenu.SetActive(true);
        }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        GameController.instance.playing = true;
        GameController.instance.Veil.enabled = false;
        pauseMenu.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
