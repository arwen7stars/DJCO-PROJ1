using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airplane : MonoBehaviour
{
    private float STOP_VELOCITY = 5f;
    private bool inFlight = false;
    private Rigidbody2D airplaneRB;
    // Use this for initialization
    private Vector2 initVelocity = Vector2.zero;
    private int swerve = 0;
    private float randomSwerve;
    void Start()
    {
        airplaneRB = GetComponent<Rigidbody2D>();
        randomSwerve = Random.Range(STOP_VELOCITY, START_VELOCITY);
        swerve = Random.Range(1, 4);
    }

    // Update is called once per frame
    void Update()
    {
        if (inFlight)
        {
            float currentVelocity = Mathf.Sqrt(Mathf.Pow(airplaneRB.velocity.x, 2) + Mathf.Pow(airplaneRB.velocity.y, 2));
            if (initVelocity == Vector2.zero)
            {
                initVelocity = airplaneRB.velocity;
                updateRotation();
            }

            if (currentVelocity < randomSwerve && swerve > 0)
            {
                float r = Random.Range(-currentVelocity / 2, currentVelocity / 2);
                airplaneRB.AddForce((new Vector2(initVelocity.y, -initVelocity.x) / START_VELOCITY) * r, ForceMode2D.Impulse);
                swerve--;
                randomSwerve = Random.Range(STOP_VELOCITY, currentVelocity);
                updateRotation();
            }

            if (currentVelocity < STOP_VELOCITY)
            {
                airplaneRB.AddForce(new Vector2(-airplaneRB.velocity.x, -airplaneRB.velocity.y) / 3, ForceMode2D.Impulse);
            }
        }
    }

    public void ResetAirplane()
    {
        inFlight = false;
        swerve = Random.Range(1, 4);
        initVelocity = Vector2.zero;
        airplaneRB.velocity = Vector2.zero;
        airplaneRB.angularVelocity = 0;
        randomSwerve = Random.Range(STOP_VELOCITY, START_VELOCITY);
    }

    private void updateRotation()
    {
        Vector2 dir = airplaneRB.velocity;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }

    public float getStartVelocity()
    {
        return START_VELOCITY;
    }

    public float getStopVelocity()
    {
        return STOP_VELOCITY;
    }

    public bool getInFlight()
    {
        return inFlight;
    }

    public void setInFlight(bool inFlight)
    {
        this.inFlight = inFlight;
    }
}