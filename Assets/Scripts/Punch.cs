using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour 
{
	private const KeyCode ACTIVATE_KEY_P1 = KeyCode.Space;
    private const KeyCode ACTIVATE_KEY_P2 = KeyCode.KeypadEnter;
    private const float COOLDOWN_TIME = 3.0f;
	private KeyCode activateKey;

    public int force;
    private bool pushing = false;

    private float cooldown = COOLDOWN_TIME;
    private bool abilityActivated = false;

    public GameObject kickSprite;

	public GameObject player;
    public Collider2D playerCollider;

	public GameObject enemyPlayer;
	private Rigidbody2D enemyPlayerRB;
    private Vector2 collisionPlayer = Vector2.zero;
    private bool canKickPlayer = false;

	public GameObject enemyAirplane;
	private Rigidbody2D enemyAirplaneRB;
    private Collider2D enemyAirplaneCollider;
    private bool canKickAirplane = false;

	void Start () 
	{
        playerCollider = this.GetComponent<Collider2D>();
        enemyPlayerRB = enemyPlayer.GetComponent<Rigidbody2D>();
		enemyAirplaneRB = enemyAirplane.GetComponent<Rigidbody2D>();
        enemyAirplaneCollider = enemyAirplane.GetComponent<Collider2D>();

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
            checkForObjects();
            
            if (Input.GetKey(activateKey) && !abilityActivated)
            {
                if (!collisionPlayer.Equals(Vector2.zero) && !enemyPlayer.GetComponent<Punch>().pushing)
                {
                    kickPlayer();
                }

                if (playerCollider.IsTouching(enemyAirplaneCollider))
				{
                    kickAirplane();
				}
            }

            if (abilityActivated)
            {
                cooldown -= Time.deltaTime;

                if (cooldown <= 0f)
                {
                    abilityActivated = false;
                    cooldown = COOLDOWN_TIME;
                }
            }

            if (Input.GetKeyUp(activateKey))
            {
                kickSprite.SetActive(false);
                pushing = false;
            }
        }
    }

    void checkForObjects()
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, transform.up, 2f);
        collisionPlayer = Vector2.zero;

        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.name.Equals(enemyPlayer.name))
            {
                collisionPlayer = hit[i].point;
            }
        }

        if (!collisionPlayer.Equals(Vector2.zero))
        {
            canKickPlayer = true;
        }
        else
        {
            canKickPlayer = false;
        }

        if (playerCollider.IsTouching(enemyAirplaneCollider))
        {
            canKickAirplane = true;
        }
        else
        {
            canKickAirplane = false;
        }
    }

    void kickPlayer()
    {
        kickSprite.transform.position = new Vector3(collisionPlayer.x, collisionPlayer.y, transform.position.z);
        kickSprite.SetActive(true);

        enemyPlayerRB.AddForce(new Vector2(
        -Mathf.Sin(Mathf.Deg2Rad * player.transform.rotation.eulerAngles.z + Mathf.Deg2Rad),
        Mathf.Cos(Mathf.Deg2Rad * player.transform.rotation.eulerAngles.z + Mathf.Deg2Rad)) *
        force * 1000, ForceMode2D.Impulse);

        pushing = true;
        abilityActivated = true;
    }

    void kickAirplane()
    {
        enemyAirplaneRB.AddForce(new Vector2(
        -Mathf.Sin(Mathf.Deg2Rad * player.transform.rotation.eulerAngles.z + Mathf.Deg2Rad),
        Mathf.Cos(Mathf.Deg2Rad * player.transform.rotation.eulerAngles.z + Mathf.Deg2Rad)) *
        force * 5, ForceMode2D.Impulse);
        abilityActivated = true;
    }

    public bool getCanKickPlayer()
    {
        return canKickPlayer;
    }

    public bool getCanKickAirplane()
    {
        return canKickAirplane;
    }

    public bool getAbilityActivated()
    {
        return abilityActivated;
    }

    public float getCooldown()
    {
        return cooldown;
    }
}
