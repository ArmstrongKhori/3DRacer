using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simply put: Handles GUI stuff.
/// </summary>
public class Designer : MonoBehaviour {



    // ??? <-- ... I can automate this later.
    public Texture[] laps_tex;
    public Texture[] placement_tex;
    public Texture tex_finish;
    public Texture2D speedometer;
    public Texture2D needle;


    internal int currentLap = 0;
    internal void ChangeLap(int lapCount)
    {
        currentLap = lapCount;
    }
    // ??? <-- Probably not the best way to do this, but doesn't matter for now...

    /// <summary>
    /// Makes the speedometer tween slowly (rather than bouncing erratically).
    /// </summary>
    internal float speedFactor;

    private void OnGUI()
    {
        GameManager gm = GameManager.Instance();
        
        
        // *** Draw GUI... *shrug*
        if (gm.isRaceDone)
        {
            GUI.DrawTexture(new Rect(Screen.width / 2 - 600 / 2, Screen.height / 2 - 200 / 2 - 100, 600, 200), tex_finish);
            GUI.DrawTexture(new Rect(Screen.width / 2 - 600 / 2, Screen.height / 2 - 200 / 2 + 100, 600, 200), placement_tex[gm.playerCar.input.placing]);
        }
        else
        {
            GUI.DrawTexture(new Rect(0, 0, 175, 75), placement_tex[gm.playerCar.input.placing]);

            GUI.DrawTexture(new Rect(Screen.width - 300, 0, 300, 100), laps_tex[currentLap]);

            GUI.DrawTexture(new Rect(Screen.width - 300, Screen.height - 150, 300, 150), speedometer);
            float rotationAngle = Mathf.Lerp(0, 180, Mathf.Abs(speedFactor));
            GUIUtility.RotateAroundPivot(rotationAngle, new Vector2(Screen.width - 150, Screen.height));
            GUI.DrawTexture(new Rect(Screen.width - 300, Screen.height - 150, 300, 300), needle);
        }

    }

    private void FixedUpdate()
    {
        GameManager gm = GameManager.Instance();
        speedFactor = Mathf.Lerp(speedFactor, gm.playerCar.currentSpeed / gm.playerCar.topSpeed, Time.deltaTime);
    }


    public static Designer instance;
    public static Designer Instance() { return instance; }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        //
        DontDestroyOnLoad(this.gameObject);
    }
}
