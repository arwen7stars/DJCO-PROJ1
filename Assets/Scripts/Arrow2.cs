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
		transform.Rotate(0, 0, turnRate*rotation*Time.deltaTime);

		if(gameObject.activeSelf)
			CheckKey();
	}

	public void ToggleActive()
	{
		if(gameObject.activeSelf)
			gameObject.SetActive(false);
		else
			gameObject.SetActive(true);
	}

	void CheckKey()
    {
        rotation = 0;

        if (Input.GetKey(KeyCode.D))
            rotation -= turnRate;
        if (Input.GetKey(KeyCode.A))
            rotation += turnRate;
    }
}
