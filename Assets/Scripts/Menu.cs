using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {
    public GameObject menuPanel;
    public GameObject menu;
    public static bool stopGame = false;
	// Use this for initialization
	void Start () {
        Color c = menu.GetComponent<Image>().color;
        menu.GetComponent<Image>().color = new Color(c.r, c.g, c.b, 255);
	}
	
	// Update is called once per frame
	void Update () {
        if (!FinishingLine.gameOver)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                if (!menuPanel.activeSelf)
                {
                    stopGame = true;
                    menuPanel.SetActive(true);
                    StopAllAnimations();
                    Time.timeScale = 0;
                }
                else
                {
                    stopGame = false;
                    menuPanel.SetActive(false);
                    StartAllAnimations();
                    Time.timeScale = 1;
                }
            }
        }
	}

    void StopAllAnimations()
    {
        Animator[] animatorsInTheScene = FindObjectsOfType(typeof(Animator)) as Animator[];
        foreach (Animator animatorItem in animatorsInTheScene)
        {
            animatorItem.enabled = false;
        }
    }

    void StartAllAnimations()
    {
        Animator[] animatorsInTheScene = FindObjectsOfType(typeof(Animator)) as Animator[];
        foreach (Animator animatorItem in animatorsInTheScene)
        {
            animatorItem.enabled = true;
        }
    }


    public void Continue()
    {
        stopGame = false;
        menuPanel.SetActive(false);
        StartAllAnimations();
        Time.timeScale = 1;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
