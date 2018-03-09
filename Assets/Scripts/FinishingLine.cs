using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishingLine : MonoBehaviour {
    public GameObject finishLine;
    public GameObject winPlayerOne;
    public GameObject winPlayerTwo;
    public GameObject tie;
    public GameObject playerOne;
    public GameObject playerTwo;

    public Collider2D airplaneOneCollider;
    public Collider2D airplaneTwoCollider;
    private Collider2D finishLineCollider;

    public static bool gameOver = false;
    public static GameObject winner = null;

	// Use this for initialization
	void Start () {
        finishLineCollider = finishLine.GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (airplaneOneCollider.IsTouching(finishLineCollider) && airplaneTwoCollider.IsTouching(finishLineCollider) && !gameOver)
        {
            tie.SetActive(true);
            gameOver = true;
        }
        
        if(!gameOver) {
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
	}
}
