using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {
    public GameObject menuPanel;
    public GameObject menu;
    public FinishingLine finishingLine;

    private bool stopGame = false;
    private const float FULL_OPACITY = 255f;

	// Use this for initialization
	void Start () {
        Color c = menu.GetComponent<Image>().color;
        menu.GetComponent<Image>().color = new Color(c.r, c.g, c.b, FULL_OPACITY);
	}
	
	// Update is called once per frame
	void Update () {
        if (!finishingLine.getGameOver())
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                if (!menuPanel.activeSelf)
                {
                    stopGame = true;
                    menuPanel.SetActive(true);
                    Time.timeScale = 0;
                }
                else
                {
                    stopGame = false;
                    menuPanel.SetActive(false);
                    Time.timeScale = 1;
                }
            }
        }
	}

    public void Continue()
    {
        stopGame = false;
        menuPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public bool getStopGame()
    {
        return stopGame;
    }
}
