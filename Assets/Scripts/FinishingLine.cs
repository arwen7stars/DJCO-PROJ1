using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishingLine : MonoBehaviour {
    public GameObject winPlayerOne;
    public GameObject winPlayerTwo;
    public GameObject tie;

    public GameObject playerOne;
    public GameObject playerTwo;

    public GameObject happy;
    public GameObject sad;

    public GameObject panelExitGame;
    public GameObject menuExitGame;

    public GameObject countdown;
    private bool musicPlaying = false;

    private Collider2D airplaneOneCollider;
    private Collider2D airplaneTwoCollider;
    private Collider2D finishLineCollider;

    private bool gameOver = false;
    private bool gameTie = false;
    private string winner = "";

    private const float FULL_OPACITY = 255f;

	// Use this for initialization
	void Start () {
        airplaneOneCollider = playerOne.GetComponent<Player>().getAirplane().GetComponent<Collider2D>();
        airplaneTwoCollider = playerTwo.GetComponent<Player>().getAirplane().GetComponent<Collider2D>();
        finishLineCollider = GetComponent<Collider2D>();

        Color c = menuExitGame.GetComponent<Image>().color;
        menuExitGame.GetComponent<Image>().color = new Color(c.r, c.g, c.b, FULL_OPACITY);
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

                panelExitGame.SetActive(true);

                if (Input.GetKey(KeyCode.Return))
                {
                    Application.Quit();
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

    public bool getGameOver()
    {
        return gameOver;
    }

    public string getWinner()
    {
        return winner;
    }

    public bool getGameTie()
    {
        return gameTie;
    }
}
