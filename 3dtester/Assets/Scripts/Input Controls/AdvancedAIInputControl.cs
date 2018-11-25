using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An enhancement on the Basic AI input control.
/// This AI will have quirks and personality, causing it to behave irregularly from ordinary AI.
/// </summary>
public class AdvancedAIInputControl : BasicAIInputControl
{

    /// <summary>
    /// How much it will err when performing precise actions
    /// IE: Turns, rebalancing, etc...
    /// </summary>
    public float whimsy = 0.0f;
    /// <summary>
    /// How much SLOWER (that's right, slower), it will go when it is at a higher ranking.
    /// The car should be made faster to compensate for this.
    /// </summary>
    public float determination = 0.0f;
    /// <summary>
    /// How eagerly it will hit the brakes (rather than swirve away) when met with walls or other cars
    /// </summary>
    public float reluctance = 0.0f;


    public override void Read(Car c)
    {
        base.Read(c);
        //
        // ??? <-- Do this.
    }
}
