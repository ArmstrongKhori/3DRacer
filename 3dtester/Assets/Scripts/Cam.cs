using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An object identified as being the camera's location.
/// It is basically "Camera 2.0".
/// </summary>
public class Cam : MonoBehaviour {

    /// <summary>
    /// The car this camera (should be) attached to.
    /// </summary>
    public Car myCar;
    /// <summary>
    /// The location of the car's tail-- Or rather, the place where the camera hovers as it follows the car.
    /// </summary>
    internal POI carTail;
    //
    /// <summary>
    /// The speed that the camera shifts from close to far.
    /// </summary>
    internal float motionFollow = 10f;
    /// <summary>
    /// A limiter for the "engage".
    /// Prevents the "engage" from increasing past this number, preventing the camera from wobbling.
    /// </summary>
    internal float motionCutoff = 0.8f;
    /// <summary>
    /// The amount the "engage" will shift over the course of one second.
    /// </summary>
    internal float motionRate = 1.0f;
    /// <summary>
    /// How far away the camera shifts from the car as "engage" approaches maximum.
    /// </summary>
    internal float motionDist = 0.3f;
    /// <summary>
    /// A silly name for the value that determines how close/far the camera hangs from the car.
    /// It tends to increase as the car goes faster.
    /// </summary>
    internal float motionEngage = 0.0f;

    private void Start()
    {
        // *** Locate the car's tail.
        carTail = myCar.transform.Find("Caming_Tail").GetComponent<POI>();
    }


    // Update is called once per frame
    void FixedUpdate () {
        Vector3 startPoint = carTail.transform.position;


        // *** ... Complicated stuff here.
        // Basically: "get the percentage of current speed to maximum speed, then *slowly* shift the 'engage' towards that amount, causing the camera to move away as you go faster."
        float diff = Mathf.Min(Mathf.Abs(myCar.currentSpeed) / myCar.topSpeed, motionCutoff) - motionEngage;
        motionEngage += Mathf.Min(Mathf.Abs(diff), motionRate * Time.deltaTime) * Mathf.Sign(diff);
        //
        // *** Basically: "*tween* the camera towards its desired position rather than snapping it straight onto there"
        transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.LerpUnclamped(myCar.transform.position, startPoint, 1 + motionDist * motionEngage), motionFollow * Time.deltaTime);
        //
        // *** ... Oh yeah, and always face the car-- Always.
        transform.LookAt(myCar.transform.position);
    }
}
