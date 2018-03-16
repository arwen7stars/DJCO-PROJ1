using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public Arrow arrow;
    public GameObject airplane;

    private const float INITIAL_SPEED = 15.0f;

    private float moveSpeed = INITIAL_SPEED;
    private float maxSpeed = 100.0f;
    private float acceleration = 5.0f;
    private float turnRate = 350f;

    private Animator playerAnimator;
    private Rigidbody2D playerRB;
    private Collider2D playerCollider;
    private bool hasAirplane = true;
    private bool running = false;
    private bool backwards = false;
    private bool finishingPosition = false;

    private Rigidbody2D airplaneRB;
    private Collider2D airplaneCollider;

    private KeyCode up;
    private KeyCode down;
    private KeyCode right;
    private KeyCode left;

    // Use this for initialization
    void Start()
    {   
        airplaneRB = airplane.GetComponent<Rigidbody2D>();
        airplaneCollider = airplane.GetComponent<BoxCollider2D>();
        playerCollider = GetComponent<Collider2D>();
        playerRB = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        Physics2D.IgnoreCollision(playerCollider, airplane.GetComponent<PolygonCollider2D>(), true);

        airplane.transform.position = new Vector3(transform.GetChild(2).position.x, transform.GetChild(2).position.y, transform.position.z);
        airplane.transform.rotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {        
        
        if (TrackTargets.gameStart)
        {
            if (!FinishingLine.gameOver)
            {
                CheckKey();

                if (hasAirplane)
                {
                    airplane.transform.position = new Vector3(transform.GetChild(2).position.x, transform.GetChild(2).position.y, transform.position.z);
                    airplane.transform.rotation = transform.rotation;
                }
                else
                {
                    catchAirplane();
                }
            }
            else
            {
                
                
                if ((FinishingLine.winner.Equals(gameObject.name) || FinishingLine.gameTie) && !finishingPosition)
                {
                    runTowardsAirplane();
                }
                else
                {
                    if (running)
                    {
                        playerAnimator.SetBool("RunToggle", false);
                    }

                    if (hasAirplane)
                    {
                        airplane.transform.position = new Vector3(transform.GetChild(2).position.x, transform.GetChild(2).position.y, transform.position.z);
                        airplane.transform.rotation = transform.rotation;
                    }
                }
            }
        }
    }
    void CheckKey()
    {
        if (this.gameObject.name.Equals("Player1"))
        {
            up = KeyCode.W;
            down = KeyCode.S;
            right = KeyCode.D;
            left = KeyCode.A;
        }
        else
        {
            up = KeyCode.UpArrow;
            down = KeyCode.DownArrow;
            right = KeyCode.RightArrow;
            left = KeyCode.LeftArrow;
        }

        if (!arrow.sRenderer.enabled) {
            if (Input.GetKey(up) && !hasAirplane)
            {
                playerAnimator.SetBool("RunToggle", true);
                running = true;
                playerRB.MovePosition(transform.position + moveSpeed * transform.up * Time.deltaTime);
                moveSpeed += acceleration * Time.deltaTime;

                if (moveSpeed > maxSpeed)
                    moveSpeed = maxSpeed;
            }
            else
            {
                if (running && !Input.GetKey(down))
                {
                    playerAnimator.SetBool("RunToggle", false);
                }
            }

            if (Input.GetKeyUp(up))
            {
                moveSpeed = INITIAL_SPEED;
            }

            if (Input.GetKey(down) && !hasAirplane)
            {
                running = true;
                backwards = true;
                playerAnimator.SetBool("RunToggle", true);
                playerRB.MovePosition(transform.position - moveSpeed * transform.up * Time.deltaTime);
            }
            else
            {
                backwards = false;
                if (running && !Input.GetKey(up))
                {
                    playerAnimator.SetBool("RunToggle", false);
                }
            }

            if (Input.GetKey(right))
            {
                playerRB.MoveRotation(playerRB.rotation - turnRate * Time.deltaTime);

                if (moveSpeed > INITIAL_SPEED)
                {
                    moveSpeed -= acceleration * Time.deltaTime;
                }

            }

            if (Input.GetKey(left))
            {
                playerRB.MoveRotation(playerRB.rotation + turnRate * Time.deltaTime);

                if (moveSpeed > INITIAL_SPEED)
                {
                    moveSpeed -= acceleration * Time.deltaTime;
                }
            }
        }
    }

    void catchAirplane()
    {
        if (Mathf.Abs(airplaneRB.velocity.x) < 2 && Mathf.Abs(airplaneRB.velocity.y) < 2)
        {
            if (playerCollider.IsTouching(airplaneCollider))
            {
                hasAirplane = true;
                airplane.GetComponent<Airplane>().ResetAirplane();
            }
        }
    }

    void runTowardsAirplane()
    {
        if (playerCollider.IsTouching(airplaneCollider))
        {
            airplane.GetComponent<Airplane>().ResetAirplane();
            playerAnimator.SetBool("RunToggle", false);

            airplane.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
            airplane.transform.rotation = transform.rotation;

            playerRB.velocity = Vector2.zero;
            playerRB.angularVelocity = 0;

            finishingPosition = true;
        }
        else
        {
            playerAnimator.SetBool("RunToggle", true);

            Vector3 target = airplane.transform.position;
            float step = moveSpeed / 2 * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target, step);

            Vector3 dir = target - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(-90 + angle, Vector3.forward);
        }
    }

    public void throwAirplane(float angle, float force)
    {
        if (hasAirplane)
        {
            hasAirplane = false;
            airplane.GetComponent<Airplane>().setInFlight(true);
            airplaneRB.AddForce(new Vector2(
                -Mathf.Sin(Mathf.Deg2Rad * transform.rotation.eulerAngles.z + Mathf.Deg2Rad * angle),
                Mathf.Cos(Mathf.Deg2Rad * transform.rotation.eulerAngles.z + Mathf.Deg2Rad * angle)) *
                airplane.GetComponent<Airplane>().getStartVelocity() * force,
                ForceMode2D.Impulse);
        }
    }

    public bool getHasAirplane() {
        return hasAirplane;
    }

    public GameObject getAirplane()
    {
        return airplane;
    }

    public bool getBackwards()
    {
        return backwards;
    }

    public bool getRunning()
    {
        return running;
    }
}
