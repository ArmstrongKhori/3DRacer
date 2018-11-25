using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Just your ordinary, run-of-the-mill helper class.
/// </summary>
public static class Helper {

    public const int SECOND = 60; // ??? <-- ... I think???


    // *** Layer IDs
    public const int LAYER_CAR = 9;
    public const int LAYER_WAYPOINT = 10;
    public const int LAYER_ZONE = 11;
    //
    // *** Bit-shifted layer IDs (in case I need to do ray-casting)
    public const int LAYER_BS_CAR = 1 << LAYER_CAR;
    public const int LAYER_BS_WAYPOINT = 1 << LAYER_WAYPOINT;
    public const int LAYER_BS_ZONE = 1 << LAYER_ZONE;

}
