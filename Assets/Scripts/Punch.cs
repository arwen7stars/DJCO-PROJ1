using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour
{
    private const KeyCode ACTIVATE_KEY_P1 = KeyCode.E;
    private const KeyCode ACTIVATE_KEY_P2 = KeyCode.RightShift;
    private const float COOLDOWN_TIME = 3.0f;
    private KeyCode activateKey;

    public int force;
    private float cooldown = COOLDOWN_TIME;
    private bool abilityActivated = false;

    public GameObject kickSprite;

    public GameObject player;
    private Collider2D playerCollider;

    public GameObject enemyPlayer;
    private Rigidbody2D enemyPlayerRB;
    private Collider2D enemyPlayerCollider;
    private Vector2 collisionPlayer = Vector2.zero;
    private bool canKickPlayer = false;

    public GameObject enemyAirplane;
    private Rigidbody2D enemyAirplaneRB;
    private Collider2D enemyAirplaneCollider;
    private Vector2 collisionAirplane = Vector2.zero;

    private bool canKickAirplane = false;

    void Start()
    {
        playerCollider = this.GetComponent<Collider2D>();

        enemyPlayerRB = enemyPlayer.GetComponent<Rigidbody2D>();
        enemyPlayerCollider = enemyPlayer.GetComponent<Collider2D>();

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
    void Update()
    {
        if (!FinishingLine.gameOver && !Menu.stopGame)
        {
            processInput();
        }
    }

    void kickPlayer()
    {
        collisionPlayer = this.transform.position;

        kickSprite.transform.position = new Vector3(collisionPlayer.x, collisionPlayer.y, transform.position.z);
        kickSprite.GetComponent<SpriteRenderer>().enabled = true;
        kickSprite.GetComponent<AudioSource>().Play();

        enemyPlayerRB.AddForce(new Vector2(
        -Mathf.Sin(Mathf.Deg2Rad * player.transform.rotation.eulerAngles.z + Mathf.Deg2Rad),
        Mathf.Cos(Mathf.Deg2Rad * player.transform.rotation.eulerAngles.z + Mathf.Deg2Rad)) *
        force * 1000, ForceMode2D.Impulse);

        abilityActivated = true;
    }

    void kickAirplane()
    {
        collisionAirplane = this.transform.position;
        kickSprite.transform.position = new Vector3(collisionAirplane.x, collisionAirplane.y, transform.position.z);
        kickSprite.GetComponent<SpriteRenderer>().enabled = true;
        kickSprite.GetComponent<AudioSource>().Play();

        enemyAirplaneRB.AddForce(new Vector2(
        -Mathf.Sin(Mathf.Deg2Rad * player.transform.rotation.eulerAngles.z + Mathf.Deg2Rad),
        Mathf.Cos(Mathf.Deg2Rad * player.transform.rotation.eulerAngles.z + Mathf.Deg2Rad)) *
        force * 5, ForceMode2D.Impulse);
        abilityActivated = true;
    }
    void processInput()
    {
        if (TrackTargets.gameStart)
        {
            checkForObjects();

            if (Input.GetKeyDown(activateKey) && !abilityActivated)
            {
                if (playerCollider.IsTouching(enemyPlayerCollider) && !player.GetComponent<Player>().getHasAirplane())
                {
                    kickPlayer();
                }

                if (playerCollider.IsTouching(enemyAirplaneCollider) && !enemyPlayer.GetComponent<Player>().getHasAirplane())
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
                kickSprite.GetComponent<SpriteRenderer>().enabled = false;

            }
        }
    }

    void checkForObjects()
    {
        if (playerCollider.IsTouching(enemyPlayerCollider) && !abilityActivated && !player.GetComponent<Player>().getHasAirplane())
        {
            canKickPlayer = true;
        }
        else
        {
            canKickPlayer = false;
        }

        if (playerCollider.IsTouching(enemyAirplaneCollider) && !abilityActivated && !enemyPlayer.GetComponent<Player>().getHasAirplane())
        {
            canKickAirplane = true;
        }
        else
        {
            canKickAirplane = false;
        }
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
