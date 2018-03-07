using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

	// the diff between the player and this object's angle
	public const int ANGLE_DIFF = 90;

	// abs value of rotation speed in angle/sec
	public const int ROTATION_SPEED = 120;

	// max/min values for offset angle
	public const int ANGLE_OFFSET_LIMIT = 30;

	// the keybind to activate the arrow
	public const KeyCode ACTIVATE_KEY = KeyCode.E;

	// the player object
	public GameObject player;

	// sprite renderer
	public SpriteRenderer sRenderer;

	// the current angle (relative to player)
	public float angle = 0;

	// the current rotation speed
    public float rotationSpeed = ROTATION_SPEED;



	void Start ()
	{
		sRenderer = GetComponent<SpriteRenderer>();

		// Start the arrow at the same position as the player.
        updateTransform();
	}

	
	// updates transform according to player
	void updateTransform() {
		transform.position = player.transform.position;
		transform.rotation = player.transform.rotation;
		transform.Rotate(0, 0, ANGLE_DIFF);
	}
	

	void Update () 
	{
		processInput();

		// no need to update stuff if object isnt visible
		if (!sRenderer.enabled) {
			return;
		}

		// update transform to match player's
		updateTransform();
		
		// update current angle
		angle += rotationSpeed * Time.deltaTime;

		// validate new angle
		while (angle < -1 * ANGLE_OFFSET_LIMIT || angle > ANGLE_OFFSET_LIMIT) {
			
			float diff = Mathf.Abs(ANGLE_OFFSET_LIMIT - Mathf.Abs(angle));		

			if (angle > ANGLE_OFFSET_LIMIT) {
				angle -= diff;
			} else if (angle < -1 * ANGLE_OFFSET_LIMIT) {
				angle += diff;
			}

			// swap rotation direction
			rotationSpeed = -1 * rotationSpeed;
		}	

		// roatate by new angle
		transform.Rotate(0, 0, angle);
	}


	void processInput() {

		/*if (Input.GetKeyDown(ACTIVATE_KEY)) {

			sRenderer.enabled = !sRenderer.enabled;

			// TODO launch paperplane
			if (!sRenderer.enabled) {
				Debug.Log("Horizontal Angle: " + angle);
			}
		}*/
	}
}
