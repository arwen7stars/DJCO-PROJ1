using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishingLine : MonoBehaviour {
    public GameObject finishLine;

    public GameObject winPlayerOne;
    public GameObject winPlayerTwo;
    public GameObject tie;

    public GameObject playerOne;
    public GameObject playerTwo;

    public GameObject happy;
    public GameObject sad;

    private Collider2D airplaneOneCollider;
    private Collider2D airplaneTwoCollider;
    private Collider2D finishLineCollider;

    public float transitionSpeed = 0.01f;
    private float opacityHappy = 0f;
    private float opacitySad = 0f;

    public static bool gameOver = false;
    public static GameObject winner = null;

	// Use this for initialization
	void Start () {
        airplaneOneCollider = playerOne.GetComponent<Player>().getAirplane().GetComponent<Collider2D>();
        airplaneTwoCollider = playerTwo.GetComponent<Player>().getAirplane().GetComponent<Collider2D>();
        finishLineCollider = finishLine.GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () {        
        if(!gameOver) {
            if (airplaneOneCollider.IsTouching(finishLineCollider) && airplaneTwoCollider.IsTouching(finishLineCollider))
            {
                tie.SetActive(true);
                gameOver = true;
            }

            if (airplaneOneCollider.IsTouching(finishLineCollider))
            {
                winPlayerOne.SetActive(true);
                winner = playerOne;
                gameOver = true;
            } else if (airplaneTwoCollider.IsTouching(finishLineCollider)){
                winPlayerTwo.SetActive(true);
                winner = playerTwo;
                gameOver = true;
            }
        }
        else
        {
            if (winner.Equals(playerOne))
            {
                showPlayerFaces(playerOne.transform, playerTwo.transform);
            }
            else
            {
                showPlayerFaces(playerTwo.transform, playerOne.transform);
            }
        }
	}

    void showPlayerFaces(Transform winner, Transform loser)
    {
        if (opacityHappy < 1f)
        {
            opacityHappy += transitionSpeed * Time.deltaTime;
            showGradually(happy, opacityHappy);
        }

        happy.transform.position = new Vector3(winner.position.x, winner.position.y + 3.5f, winner.position.z - 1f);

        if (opacitySad < 1f)
        {
            opacitySad += transitionSpeed * Time.deltaTime;
            showGradually(sad, opacitySad);
        }

        sad.transform.position = new Vector3(loser.position.x, loser.position.y + 3.5f, loser.position.z - 1f);    
    }

    public static void showGradually(GameObject obj, float opacity)
    {
        Color c = obj.GetComponent<SpriteRenderer>().color;
        c = new Color(c.r, c.g, c.b, opacity);
        obj.SetActive(true);
    }
}
