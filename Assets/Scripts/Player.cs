using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public Arrow arrow;
    public GameObject airplane;

    private float moveSpeed = 6.0f;
    private float maxSpeed = 12.0f;
    private float acceleration = 1.0f;
    private float turnRate = 250f;

    private Animator playerAnimator;
    private Rigidbody2D playerRB;
    private Collider2D playerCollider;
    private bool hasAirplane = true;
    private bool running = false;
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
        airplaneCollider = airplane.GetComponent<Collider2D>();
        playerCollider = GetComponent<Collider2D>();
        playerRB = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        Physics2D.IgnoreCollision(playerCollider, airplaneCollider, true);
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
                    Physics2D.IgnoreCollision(playerCollider, airplaneCollider, true);
                    airplane.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
                    airplane.transform.rotation = transform.rotation;
                }
                else
                {
                    if (Mathf.Abs(airplaneRB.velocity.x) < 2 && Mathf.Abs(airplaneRB.velocity.y) < 2)
                    {
                        Physics2D.IgnoreCollision(playerCollider, airplaneCollider, false);
                        if (playerCollider.IsTouching(airplaneCollider))
                        {
                            hasAirplane = true;
                            airplane.GetComponent<Airplane>().ResetAirplane();
                        }
                    }
                    else Physics2D.IgnoreCollision(playerCollider, airplaneCollider, true);
                }
                playerRB.velocity = Vector2.zero;
                playerRB.angularVelocity = 0;
            }
            else
            {
                Physics2D.IgnoreCollision(playerCollider, airplaneCollider, true);

                if (FinishingLine.winner.name.Equals(gameObject.name) && !finishingPosition)
                {
                    Physics2D.IgnoreCollision(playerCollider, airplaneCollider, false);

                    if (playerCollider.IsTouching(airplaneCollider))
                    {
                        airplane.GetComponent<Airplane>().ResetAirplane();
                        playerAnimator.SetBool("RunToggle", false);

                        Physics2D.IgnoreCollision(playerCollider, airplaneCollider, true);

                        airplane.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
                        airplane.transform.rotation = transform.rotation;

                        finishingPosition = true;
                    }
                    else
                    {
                        playerAnimator.SetBool("RunToggle", true);

                        Vector3 target = airplane.transform.position;
                        float step = moveSpeed / 2 * Time.deltaTime;
                        transform.position = Vector3.MoveTowards(transform.position, target, step);
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
                moveSpeed = 6.0f;

                if (running && !Input.GetKey(down))
                {
                    playerAnimator.SetBool("RunToggle", false);
                }
            }

            if (Input.GetKey(down) && !hasAirplane)
            {
                playerAnimator.SetBool("RunToggle", true);
                playerRB.MovePosition(transform.position - moveSpeed * transform.up * Time.deltaTime);
            }
            else
            {
                if (running && !Input.GetKey(up))
                {
                    playerAnimator.SetBool("RunToggle", false);
                }
            }

            if (Input.GetKey(right))
            {
                playerRB.MoveRotation(playerRB.rotation + turnRate * Time.fixedDeltaTime);
            }

            if (Input.GetKey(left))
            {
                playerRB.MoveRotation(playerRB.rotation - turnRate * Time.fixedDeltaTime);
            }
        }
    }

    public void throwAirplane(float angle)
    {
        if (hasAirplane)
        {
            hasAirplane = false;
            airplane.GetComponent<Airplane>().setInFlight(true);
            airplaneRB.AddForce(new Vector2(
                -Mathf.Sin(Mathf.Deg2Rad * transform.rotation.eulerAngles.z + Mathf.Deg2Rad * angle),
                Mathf.Cos(Mathf.Deg2Rad * transform.rotation.eulerAngles.z + Mathf.Deg2Rad * angle)) *
                airplane.GetComponent<Airplane>().getStartVelocity(),
                ForceMode2D.Impulse);
        }
    }

    public bool getHasAirplane() {
        return hasAirplane;
    }

}
