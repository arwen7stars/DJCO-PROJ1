using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour 
{
	public int x;
	public int y;
	public int force;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerStay2D(Collider2D other) 
	{
        if(other.gameObject.name.Contains("Player"))
		{
			Rigidbody2D rigidbody = other.gameObject.GetComponent<Rigidbody2D>();
			rigidbody.AddForce(new Vector2(x, y) * force * 100, ForceMode2D.Force);
		}
		else if(other.gameObject.name.Contains("Airplane"))
		{
			Rigidbody2D rigidbody = other.gameObject.GetComponent<Rigidbody2D>();
			rigidbody.AddForce(new Vector2(x, y) * force, ForceMode2D.Force);
		}
    }
}
