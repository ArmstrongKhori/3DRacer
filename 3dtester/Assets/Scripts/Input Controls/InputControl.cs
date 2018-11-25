using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An empty input control that feeds absolutely nothing.
/// </summary>
public class InputControl : MonoBehaviour {


    // ??? <-- This is NOT the place to put this stuff, but eh... It works for now.
    internal int currentWaypoint;
    internal int lapCount = 0;
    /// <summary>
    /// What place are you?
    /// </summary>
    internal int placing = 1;

    internal bool inputBrake = false; // *** Braking; causes the car to slow down
    internal bool inputReset = false; // *** Resets the car to its last waypoint
    internal float inputTorque = 0.0f; // *** Go forward or backward
    internal float inputSteer = 0.0f; // *** Steer left or right

    internal bool isAIControlled = true; // *** AI controlled? Necessary for certain checks.

    /// <summary>
    /// Check the input for the car.
    /// Depending on the "type" of input controller this is, the player or the AI may be manipulating the values, hence it being virtual.
    /// </summary>
    /// <param name="c">The car this input is checking with.</param>
    public virtual void Read(Car c)
    {
        inputBrake = false;
        inputReset = false;
        inputTorque = 0.0f;
        inputSteer = 0.0f;
    }

    /// <summary>
    /// Occurs when the car hits a waypoint.
    /// Important for determining placement in the race.
    /// </summary>
    /// <param name="c"></param>
    /// <param name="waypoint"></param>
    public virtual void OnWaypointReached(Car c, WaypointNode waypoint) { }
}

