using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Invisible (or perhaps visible?) boundaries that affect cars in a certain way.
/// </summary>
public class Zone : MonoBehaviour {

    public enum ZoneType
    {
        /// <summary>
        /// Does absolutely nothing.
        /// </summary>
        None,
        /// <summary>
        /// Cars are forced to reset
        /// </summary>
        OutOfBounds,
    }
    public ZoneType zoneType;



    private void Start()
    {
        gameObject.layer = Helper.LAYER_ZONE;
    }

    /// <summary>
    /// This is what occurs when a car touches a zone.
    /// </summary>
    /// <param name="c"></param>
    public void OnZoneTouched(Car c)
    {
        switch (zoneType)
        {
            case ZoneType.OutOfBounds:
                c.InvalidPosReset();
                break;
        }
    }


    private void OnTriggerStay(Collider other)
    {
        Car c = other.gameObject.GetComponent<Car>();
        if (c != null)
        {
            OnZoneTouched(c);
        }
    }


}
