using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow2 : MonoBehaviour {

	public int turnRate = 14;
	public int rotation;

	void Start ()
	{
		transform.Rotate(0, 0, 90);
		gameObject.SetActive(false);
	}
	
	void Update () 
	{
		
	}

	public void ToggleActive()
	{
		if(gameObject.activeSelf)
			gameObject.SetActive(false);
		else
			gameObject.SetActive(true);
	}
}
