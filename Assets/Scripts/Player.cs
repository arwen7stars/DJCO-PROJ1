using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public Arrow arrow;
    public GameObject airplane;

    private float moveSpeed = 6.0f;
    private float maxSpeed = 10.0f;
    private float acceleration = 1.0f;
    private float turnRate = 100f;    

    public Rigidbody2D playerRB;
    public Collider2D playerCollider;
    private bool hasAirplane = true;

    private Rigidbody2D airplaneRB;
    public Collider2D airplaneCollider;

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
        Physics2D.IgnoreCollision(playerCollider, airplaneCollider, true);
    }

    // Update is called once per frame
    void Update()
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
            
            if (FinishingLine.winner.name.Equals(gameObject.name))
            {
                Vector3 target = airplane.transform.position;
                float step = moveSpeed / 2 * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, target, step);

                if (playerCollider.IsTouching(airplaneCollider))
                {
                    airplane.GetComponent<Airplane>().ResetAirplane();
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
                playerRB.MovePosition(transform.position + moveSpeed * transform.up * Time.deltaTime);
                
                moveSpeed += acceleration * Time.deltaTime;

                if (moveSpeed > maxSpeed)
                    moveSpeed = maxSpeed;
            }
            else
            {
                moveSpeed = 6.0f;
            }

            if (Input.GetKey(down) && !hasAirplane)
            {
                playerRB.MovePosition(transform.position - moveSpeed * transform.up * Time.deltaTime);

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
