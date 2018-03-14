using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour 
{
	public const KeyCode ACTIVATE_KEY_P1 = KeyCode.Space;
	public const KeyCode ACTIVATE_KEY_P2 = KeyCode.KeypadEnter;
	public int force;
	public KeyCode activateKey;
	public GameObject player;
	private Rigidbody2D playerRB;
	public GameObject enemyPlayer;
	private Collider2D collider;
	private Collider2D enemyPlayerCollider;
	private Rigidbody2D enemyPlayerRB;
	public GameObject enemyAirplane;
	private Collider2D enemyAirplaneCollider;
	private Rigidbody2D enemyAirplaneRB;

	void Start () 
	{
		collider = GetComponent<Collider2D>();
		playerRB = player.GetComponent<Rigidbody2D>();
		enemyPlayerCollider = enemyPlayer.GetComponent<Collider2D>();
		enemyPlayerRB = enemyPlayer.GetComponent<Rigidbody2D>();
		enemyAirplaneCollider = enemyAirplane.GetComponent<Collider2D>();
		enemyAirplaneRB = enemyAirplane.GetComponent<Rigidbody2D>();

		if (player.gameObject.name.Equals("Player1")) 
		{
            activateKey = ACTIVATE_KEY_P1;
        }
        else
        {
            activateKey = ACTIVATE_KEY_P2;
        }	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!FinishingLine.gameOver)
        {
            processInput();
		}
	}

	void processInput()
    {
        if (TrackTargets.gameStart)
        {
            if (Input.GetKeyDown(activateKey))
            {
                if(collider.IsTouching(enemyPlayerCollider))
				{
					enemyPlayerRB.AddForce(new Vector2(
						-Mathf.Sin(Mathf.Deg2Rad * player.transform.rotation.eulerAngles.z + Mathf.Deg2Rad),
						Mathf.Cos(Mathf.Deg2Rad * player.transform.rotation.eulerAngles.z + Mathf.Deg2Rad)) *
						force * 1000,
						ForceMode2D.Impulse);
				}

				if(collider.IsTouching(enemyAirplaneCollider))
				{
					enemyAirplaneRB.AddForce(new Vector2(
						-Mathf.Sin(Mathf.Deg2Rad * player.transform.rotation.eulerAngles.z + Mathf.Deg2Rad),
						Mathf.Cos(Mathf.Deg2Rad * player.transform.rotation.eulerAngles.z + Mathf.Deg2Rad)) *
						force * 5,
						ForceMode2D.Impulse);
				}
            }
        }
    }
}
