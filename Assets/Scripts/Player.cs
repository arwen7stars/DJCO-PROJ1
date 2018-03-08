using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int moveSpeed = 6;
    public int turnRate = 14;
    public Arrow arrowObj;
    public int rotation;
    private Vector2 direction;
    private bool isMoving = true;
    private bool hasAirplane = true;

    public GameObject airplane;
    private Rigidbody2D airplaneRB;
	private Collider2D airplaneCollider;
    private Collider2D playerCollider;

	// Use this for initialization
	void Start ()
    {
        airplaneRB = airplane.GetComponent<Rigidbody2D>();
        airplaneCollider = airplane.GetComponent<Collider2D>();
        playerCollider = GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        CheckKey();

		if(isMoving)
        {
            transform.Translate(direction*moveSpeed*Time.deltaTime);
        }

        transform.Rotate(0, 0, turnRate*rotation*Time.deltaTime);

        if(hasAirplane)
        {
            Physics2D.IgnoreCollision(playerCollider, airplaneCollider, true);
            airplane.transform.position = transform.position;
            airplane.transform.rotation = transform.rotation;
        }
        else 
        {
            if(Mathf.Abs(airplaneRB.velocity.x) < 1 && Mathf.Abs(airplaneRB.velocity.y) < 1)
            {
                Physics2D.IgnoreCollision(playerCollider, airplaneCollider, false);
                if(playerCollider.IsTouching(airplaneCollider))
                {
                    hasAirplane = true;
                    airplane.GetComponent<Airplane>().ResetAirplane();
                }
            }
        }
	}

    void CheckKey()
    {
        direction = Vector2.zero;
        rotation = 0;
        object[] obj = GameObject.FindObjectsOfType(typeof(GameObject));
        foreach (object o in obj)
        {
            GameObject go = (GameObject)o;
            if (go.name.Equals("Arrow"))
            {
                arrowObj = go.GetComponent<Arrow>();
            }
        }

        if (Input.GetKey(KeyCode.W))
            direction += Vector2.up;
        if (Input.GetKey(KeyCode.S))
            direction += Vector2.down;
        if (Input.GetKey(KeyCode.D))
            rotation -= turnRate;
        if (Input.GetKey(KeyCode.A))
            rotation += turnRate;
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(hasAirplane)
            {
                if (arrowObj.sRenderer.enabled)
                {
                    hasAirplane = false;
                    airplane.GetComponent<Airplane>().inFlight = true;
                    airplaneRB.AddForce(new Vector2(
                        -Mathf.Sin(Mathf.Deg2Rad * transform.rotation.eulerAngles.z + Mathf.Deg2Rad * arrowObj.angle),
                        Mathf.Cos(Mathf.Deg2Rad * transform.rotation.eulerAngles.z + Mathf.Deg2Rad * arrowObj.angle)) * 
                        airplane.GetComponent<Airplane>().START_VELOCITY,
                        ForceMode2D.Impulse);
                    arrowObj.sRenderer.enabled = !arrowObj.sRenderer.enabled;
                }
                else arrowObj.sRenderer.enabled = !arrowObj.sRenderer.enabled;
            }
        }
    }
}
