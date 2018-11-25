using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputControl : InputControl
{
    private void Start()
    {
        isAIControlled = false; // *** Unlike normal AI, *WE* are controlling this!
    }

    public override void Read(Car c)
    {
        base.Read(c);
        //
        inputBrake = Input.GetButton("Jump");
        inputReset = Input.GetButton("Submit");
        inputTorque = Input.GetAxis("Vertical");
        inputSteer = Input.GetAxis("Horizontal");
    }

    public override void OnWaypointReached(Car c, WaypointNode waypoint)
    {
        base.OnWaypointReached(c, waypoint);
        //
        // *** The player is allowed to hit one waypoint ahead of their current one!
        if (waypoint.wayID == currentWaypoint || waypoint.wayID == currentWaypoint+1)
        {
            currentWaypoint++;
            //
            // *** Next lap!
            if (currentWaypoint >= waypoint.owner.Count)
            {
                lapCount++;
                currentWaypoint = 0;
                //
                GameManager.Instance().OnCarLapped(c);
            }
            //
            // *** Light up the waypoints we need to go after next.
            waypoint.owner.Illuminate(currentWaypoint);
            waypoint.owner.Illuminate(currentWaypoint + 1, false);
        }
    }
}
