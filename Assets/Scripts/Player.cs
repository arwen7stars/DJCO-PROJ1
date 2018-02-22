using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int moveSpeed = 2;
    public int turnRate = 1;

    public int rotation;
    private Vector2 direction;
    private int angle = 0;
    private bool isMoving;

	// Use this for initialization
	void Start ()
    {
        isMoving = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
        CheckKey();

		if(isMoving)
        {
            transform.Translate(direction*moveSpeed*Time.deltaTime);
            transform.Rotate(0, 0, turnRate*rotation*Time.deltaTime);
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
    }
}
