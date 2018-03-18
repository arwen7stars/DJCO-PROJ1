using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teacher : MonoBehaviour {
    public Transform[] points;
    private float speed = 1.0f;
    private int currentPoint = 0;

	// Use this for initialization
	void Start () {
        GetComponent<Animator>().SetBool("RunToggle", true);
	}
	
	
    // Update is called once per frame
    void Update()
    {
        if (currentPoint < points.Length) // this is to check if it's less than the length of the points
        {
            float dist = Vector3.Distance(transform.position, points[currentPoint].position);

            if (dist < 1f)
            {
                currentPoint++;
                //transform.Rotate(Vector3.forward, 180);
            }
            else
            {
                Vector3 dir = points[currentPoint].position - transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(-90 + angle, Vector3.forward);

                transform.position = Vector3.Lerp(this.transform.position, points[currentPoint].position, Time.deltaTime * speed);
            }
        }
        else
        {
            currentPoint = 0; //this is to loop it back to zero again
        }
    }

    public Quaternion RotateTowardsPoint(Vector3 position, Quaternion rotation, Vector3 target)
    {
        //find the vector pointing from our position to the target
        Vector3 direction = (target - position).normalized;

        //create the rotation we need to be in to look at the target
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        return Quaternion.Slerp(rotation, lookRotation, Time.deltaTime);
    }
 
}
