using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airplane : MonoBehaviour 
{
	public bool inFlight = false;
	private Rigidbody2D airplaneRB;
	// Use this for initialization
	private Vector2 initVelocity = Vector2.zero;
	void Start () 
	{
		airplaneRB = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(inFlight)
		{
			/*if(initVelocity == Vector2.zero)
			{
				initVelocity = airplaneRB.velocity;
				airplaneRB.AddForce(new Vector2(initVelocity.y, -initVelocity.x)/10,
                        ForceMode2D.Impulse);
			}*/

			Debug.Log(airplaneRB.velocity);

			if(Mathf.Abs(airplaneRB.velocity.x) < 4 && Mathf.Abs(airplaneRB.velocity.y) < 4)
			{
				airplaneRB.AddForce(new Vector2(-airplaneRB.velocity.x, -airplaneRB.velocity.y)/3,
                        ForceMode2D.Impulse);
			}
		}
	}
}
