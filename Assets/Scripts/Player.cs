using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int moveSpeed = 6;
    public int turnRate = 14;

    public int rotation;
    private Vector2 direction;
    private bool isMoving = true;
    private bool hasAirplane = true;

    public GameObject airplane;
    public Rigidbody2D airplaneRB;

    public int thrust = 10;

	// Use this for initialization
	void Start ()
    {
        airplaneRB = airplane.GetComponent<Rigidbody2D>();
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
            airplane.transform.position = transform.position;
            airplane.transform.rotation = transform.rotation;
        }
	}

    void CheckKey()
    {
        direction = Vector2.zero;
        rotation = 0;

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
                hasAirplane = false;
                airplaneRB.AddForce(new Vector2(
                    -Mathf.Sin(Mathf.Deg2Rad * transform.rotation.eulerAngles.z), 
                    Mathf.Cos(Mathf.Deg2Rad * transform.rotation.eulerAngles.z))* thrust, 
                    ForceMode2D.Impulse);
            }
        }
    }  
}
