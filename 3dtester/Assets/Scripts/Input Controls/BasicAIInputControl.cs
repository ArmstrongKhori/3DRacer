using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Automated AI control for the car.
/// This AI's objective is to make laps around the course over and over without crashing.
/// </summary>
public class BasicAIInputControl : InputControl
{
    public override void Read(Car c)
    {
        base.Read(c);
        //
        Rigidbody body = c.gameObject.GetComponent<Rigidbody>();


        GameManager gm = GameManager.Instance();


        // *** Upside-down for 3 seconds? Just reset...
        if (c.flippedTime > 3f)
        {
            inputReset = true;
        }
        // *** Immobile for 3 seconds? Just reset...
        else if (c.immobileTime > 3f)
        {
            inputReset = true;
        }
        else if (!(c.wheelBL.isGrounded || c.wheelBR.isGrounded || c.wheelFL.isGrounded || c.wheelFR.isGrounded))
        {
            if (c.transform.localEulerAngles.z > 0) { inputTorque = -1; }
            else if (c.transform.localEulerAngles.z < -0) { inputTorque = 1; }

            if (c.transform.localEulerAngles.x > 0) { inputSteer = 1; }
            else if (c.transform.localEulerAngles.x < -0) { inputSteer = -1; }
        }
        else
        {
            //calculate turn angle
            Vector3 RelativeWaypointPosition = transform.InverseTransformPoint(new Vector3(gm.waypoints[currentWaypoint].position.x, transform.position.y, gm.waypoints[currentWaypoint].position.z));
            inputSteer = RelativeWaypointPosition.x / RelativeWaypointPosition.magnitude;
            inputSteer += CheckSpacing(c);
            // inputSteer += CheckOnComing(c);

            //Spoilers add down pressure based on the car’s speed. (Upside-down lift)
            Vector3 localVelocity = transform.InverseTransformDirection(body.velocity);
            // body.AddForce(-transform.up * (localVelocity.z * c.spoilerRatio), ForceMode.Impulse);

            //calculate torque.		
            if (Mathf.Abs(inputSteer) < 0.5f)
            {
                //when making minor turning adjustments speed is based on how far to the next point.
                inputTorque = 1; // (RelativeWaypointPosition.z / RelativeWaypointPosition.magnitude);
                inputBrake = false;
            }
            else
            {
                //we need to make a hard turn, if moving fast apply handbrake to slide.
                if (body.velocity.magnitude > 10)
                {
                    inputBrake = true;
                }
                //if not moving forward backup and turn opposite.
                else if (localVelocity.z < 0)
                {
                    inputBrake = false;
                    inputTorque = -1;
                    inputSteer *= -1;
                }
                //let off the gas while making a hard turn.
                else
                {
                    inputBrake = false;
                    inputTorque = 0;
                }
            }
        }

        inputBrake = false; // ??? <-- ... Just... Don't. The cars control BETTER *without* using the brakes.
    }

    public override void OnWaypointReached(Car c, WaypointNode waypoint)
    {
        base.OnWaypointReached(c, waypoint);
        //
        // *** Reached our waypoint? Go to the next one.
        if (waypoint.wayID == currentWaypoint)
        {
            currentWaypoint++;
            //
            // *** Finished a lap? Start over again and increment our lap count.
            if (currentWaypoint >= waypoint.owner.Count)
            {
                lapCount++;
                currentWaypoint = 0;
                //
                GameManager.Instance().OnCarLapped(c);
            }
        }
    }


    // public float breakingDistance = 6f;
    public float forwardOffset;
    public float spacingDistance = 1f;
    public float sideOffset;
    public float cornerDistance = 9f;

    private float CheckSpacing(Car c)
    {
        float steeringAdjustment = 0;

        //check to our right
        RaycastHit hit;
        Vector3 carRight = transform.position + (transform.right * sideOffset);
        Debug.DrawRay(carRight, transform.right * spacingDistance);

        //if we detect a car to the right turn left.
        if (Physics.Raycast(carRight, transform.right, out hit, spacingDistance))
        {
            steeringAdjustment += -1 + ((carRight - hit.point).magnitude / spacingDistance);
        }

        //check to our left
        Vector3 carLeft = transform.position + (-transform.right * sideOffset);
        Debug.DrawRay(carLeft, -transform.right * spacingDistance);

        //if we detect a car to the left turn right.
        if (Physics.Raycast(carLeft, -transform.right, out hit, spacingDistance))
        {
            steeringAdjustment += 1 - ((carLeft - hit.point).magnitude / spacingDistance);
        }

        //otherwise no change
        return steeringAdjustment;
    }

    private float CheckOnComing(Car c)
    {
        float steeringAdjustment = 0;

        //check to our right
        RaycastHit hit;
        Vector3 carFrontRight = transform.position + (transform.right * sideOffset) + (transform.forward * forwardOffset);
        Debug.DrawRay(carFrontRight, (transform.right * 0.5f + transform.forward) * cornerDistance);

        //if we detect a car to the right turn left.
        if (Physics.Raycast(carFrontRight, (transform.right * 0.5f + transform.forward), out hit, cornerDistance))
        {
            steeringAdjustment += -1 + ((carFrontRight - hit.point).magnitude / cornerDistance);
        }

        //check to our left
        Vector3 carFrontLeft = transform.position + (-transform.right * sideOffset) + (transform.forward * forwardOffset); ;
        Debug.DrawRay(carFrontLeft, (-transform.right * 0.5f + transform.forward) * cornerDistance);

        //if we detect a car to the left turn right.
        if (Physics.Raycast(carFrontLeft, (-transform.right * 0.5f + transform.forward), out hit, cornerDistance))
        {
            steeringAdjustment += 1 - ((carFrontLeft - hit.point).magnitude / cornerDistance);
        }

        //otherwise no change
        return steeringAdjustment;
    }
}
