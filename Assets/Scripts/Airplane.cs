using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airplane : MonoBehaviour 
{
	public bool inFlight = false;
	private Rigidbody2D airplaneRB;
	// Use this for initialization
	private Vector2 initVelocity = Vector2.zero;

	private bool swerve = false;
	void Start () 
	{
		airplaneRB = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(inFlight)
		{
			float randomSwerve = 20;
			if(initVelocity == Vector2.zero)
			{
				initVelocity = airplaneRB.velocity;
				randomSwerve = Random.Range(15f, 20f);
				updatRotation();
			}

			if(Mathf.Abs(airplaneRB.velocity.x) < randomSwerve && Mathf.Abs(airplaneRB.velocity.y) < randomSwerve && !swerve)
			{
				float r = Random.Range(-3f, 3f);
				airplaneRB.AddForce((new Vector2(initVelocity.y, -initVelocity.x)/10) * r, ForceMode2D.Impulse);
				swerve = true;
				updatRotation();
			}

			if(Mathf.Abs(airplaneRB.velocity.x) < 15 && Mathf.Abs(airplaneRB.velocity.y) < 15)
			{
				airplaneRB.AddForce(new Vector2(-airplaneRB.velocity.x, -airplaneRB.velocity.y)/3, ForceMode2D.Impulse);
			}
		}
	}

	public void ResetAirplane()
	{
		inFlight = false;
		swerve = false;
		initVelocity = Vector2.zero;
		airplaneRB.velocity = Vector2.zero;
	}

	private void updatRotation()
	{
		Vector2 dir = airplaneRB.velocity;
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle-90, Vector3.forward);
	}
}
