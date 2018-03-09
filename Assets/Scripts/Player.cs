using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public Arrow arrow;
    public GameObject airplane;

    private float moveSpeed = 3.0f;
    private float maxSpeed = 10.0f;
    private float acceleration = 1.0f;

    private int turnRate = 14;    
    private int rotation;
    private Vector2 direction;
    private Rigidbody2D playerRB;
    private Collider2D playerCollider;
    private bool hasAirplane = true;
    private bool collisionDetect = false;

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
        direction = Vector2.zero;
        rotation = 0;

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
                direction += Vector2.up;
                transform.Translate(direction * moveSpeed * Time.deltaTime);
                
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
                direction += Vector2.down;
                transform.Translate(direction * moveSpeed * Time.deltaTime);
            }
            if (Input.GetKey(right))
            {
                rotation -= turnRate;
                transform.Rotate(0, 0, turnRate * rotation * Time.deltaTime);
            }
            if (Input.GetKey(left))
            {
                rotation += turnRate;
                transform.Rotate(0, 0, turnRate * rotation * Time.deltaTime);
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

    void OnCollisionEnter(Collision collision)
    {
        collisionDetect = true;
        Debug.Log(collisionDetect);
    }

    void OnCollisionExit(Collision collision)
    {
        collisionDetect = false;
        Debug.Log(collisionDetect);
    }
}
