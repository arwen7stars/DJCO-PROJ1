﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class FinishingLine : MonoBehaviour {
    public GameObject finishLine;

    public GameObject winPlayerOne;
    public GameObject winPlayerTwo;
    public GameObject tie;

    public GameObject playerOne;
    public GameObject playerTwo;

    public GameObject happy;
    public GameObject sad;

    public GameObject panel;
    public GameObject textExitGame;

    public GameObject countdown;
    private bool musicPlaying = false;

    private Collider2D airplaneOneCollider;
    private Collider2D airplaneTwoCollider;
    private Collider2D finishLineCollider;

    private float transitionSpeed = 3f;
    private float opacityExitGame = 0f;

    public static bool gameOver = false;
    public static bool gameTie = false;
    public static string winner = "";

	// Use this for initialization
	void Start () {
        airplaneOneCollider = playerOne.GetComponent<Player>().getAirplane().GetComponent<Collider2D>();
        airplaneTwoCollider = playerTwo.GetComponent<Player>().getAirplane().GetComponent<Collider2D>();
        finishLineCollider = finishLine.GetComponent<Collider2D>();
        Color c = textExitGame.GetComponent<Text>().color;

        textExitGame.GetComponent<Text>().color = new Color(c.r, c.g, c.b, 0);
	}
	
	// Update is called once per frame
	void Update () {        
        if(!gameOver) {
            if (airplaneOneCollider.IsTouching(finishLineCollider) && airplaneTwoCollider.IsTouching(finishLineCollider))
            {
                gameTie = true;
                tie.SetActive(true);
                gameOver = true;
            }

            if (!gameTie)
            {
                if (airplaneOneCollider.IsTouching(finishLineCollider))
                {
                    winPlayerOne.SetActive(true);
                    winner = playerOne.name;
                    gameOver = true;
                }
                else if (airplaneTwoCollider.IsTouching(finishLineCollider))
                {
                    winPlayerTwo.SetActive(true);
                    winner = playerTwo.name;
                    gameOver = true;
                }
            }
        }
        else
        {
            if (!musicPlaying)
            {
                countdown.GetComponent<AudioSource>().Stop();
                GetComponent<AudioSource>().Play();
                musicPlaying = true;
            }

            if (!GetComponent<AudioSource>().isPlaying)
            {
                if (winPlayerOne.activeSelf)
                {
                    winPlayerOne.SetActive(false);
                }
                else if (winPlayerTwo.activeSelf)
                {
                    winPlayerTwo.SetActive(false);
                }
                else if (tie.activeSelf)
                {
                    tie.SetActive(false);
                }

                if (opacityExitGame < 1f)
                {
                    Countdown.showGradually(panel, opacityExitGame);
                    showTextGradually(textExitGame, opacityExitGame);
                    opacityExitGame += transitionSpeed * Time.deltaTime;
                }

                if (Input.GetKey(KeyCode.Return))
                {
                    Application.Quit();
                    EditorApplication.isPlaying = false;
                }
            }

            if (!gameTie)
            {
                if (winner.Equals(playerOne.name))
                {
                    showPlayerFaces(playerOne.transform, playerTwo.transform);
                }
                else
                {
                    showPlayerFaces(playerTwo.transform, playerOne.transform);
                }
            }
            else
            {
                showHappyFace(playerOne.transform);
                showHappyFace(playerTwo.transform);
            }
        }
	}

    void showTextGradually(GameObject obj, float opacity)
    {
        Text txt = obj.GetComponent<Text>();
        Color c = txt.color;

        txt.color = new Color(c.r, c.g, c.b, opacity);
        obj.SetActive(true);
    }

    void showHappyFace(Transform winner)
    {
        happy.SetActive(true);
        happy.transform.position = new Vector3(winner.position.x, winner.position.y + 3.5f, winner.position.z - 1f);
    }

    void showSadFace(Transform loser)
    {
        sad.SetActive(true);
        sad.transform.position = new Vector3(loser.position.x, loser.position.y + 3.5f, loser.position.z - 1f);
    }
    void showPlayerFaces(Transform winner, Transform loser)
    {
        showHappyFace(winner);
        showSadFace(loser);
    }
}
