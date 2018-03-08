using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public Arrow arrow;

    public int moveSpeed = 6;
    public int turnRate = 14;    
    public int rotation;
    private Vector2 direction;
    public Rigidbody2D playerRB;
    private Collider2D playerCollider;
    public bool hasAirplane = true;

    public GameObject airplane;
    public Rigidbody2D airplaneRB;
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

    public void throwAirplane(float angle) {
        if (hasAirplane)
        {
            hasAirplane = false;
            airplane.GetComponent<Airplane>().inFlight = true;
            airplaneRB.AddForce(new Vector2(
                -Mathf.Sin(Mathf.Deg2Rad * transform.rotation.eulerAngles.z + Mathf.Deg2Rad * angle),
                Mathf.Cos(Mathf.Deg2Rad * transform.rotation.eulerAngles.z + Mathf.Deg2Rad * angle)) *
                airplane.GetComponent<Airplane>().START_VELOCITY,
                ForceMode2D.Impulse);
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerRB.angularVelocity = 0;

        CheckKey();

        if (hasAirplane)
        {
            Physics2D.IgnoreCollision(playerCollider, airplaneCollider, true);
            airplane.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
            airplane.transform.rotation = transform.rotation;
        }
        else
        {
            if (Mathf.Abs(airplaneRB.velocity.x) < 1 && Mathf.Abs(airplaneRB.velocity.y) < 1)
            {
                Physics2D.IgnoreCollision(playerCollider, airplaneCollider, false);
                if (playerCollider.IsTouching(airplaneCollider))
                {
                    hasAirplane = true;
                    airplane.GetComponent<Airplane>().ResetAirplane();
                }
                playerRB.velocity = Vector2.zero;
                playerRB.angularVelocity = 0;
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
}
