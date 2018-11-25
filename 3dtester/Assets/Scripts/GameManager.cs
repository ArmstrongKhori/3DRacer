using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the game.
/// In the context of this game, it takes care of laps, waypoints, and changing the game mode.
/// </summary>
public class GameManager : MonoBehaviour {

    public WaypointGizmos raceWaypoints;
    internal Transform[] waypoints;

    public Mesh _sphere;

    public Car[] allCars;
    

    void Initialize()
    {
        foreach (WaypointNode n in FindObjectsOfType<WaypointNode>())
        {
            n.InitializeMe();
        }
    }


    // Use this for initialization
    void Start () {

        //NOTE: Unity named this function poorly it also returns the parent’s component.
        Transform[] potentialWaypoints = raceWaypoints.GetComponentsInChildren<Transform>();

        //initialize the waypoints array so that is has enough space to store the nodes.
        waypoints = new Transform[(potentialWaypoints.Length - 1)];

        //loop through the list and copy the nodes into the array.
        //start at 1 instead of 0 to skip the WaypointContainer’s transform.
        for (int i = 1; i < potentialWaypoints.Length; ++i)
        {
            waypoints[i - 1] = potentialWaypoints[i];
        }



        allCars = FindObjectsOfType<Car>();
        //
        foreach (Car c in allCars)
        {
            if (!c.input.isAIControlled)
            {
                playerCar = c;
                break;
            }
        }
    }
    internal Car playerCar;

    internal void OnCarLapped(Car c)
    {
        if (isRaceDone) { return; }

        if (!c.input.isAIControlled)
        {
            if (c.input.lapCount < 3)
            {
                Designer.Instance().ChangeLap(c.input.lapCount);
            }
            else
            {
                // *** Become automatically controlled-- You're done!
                Game_ConcludeRace();
                //
                BasicAIInputControl ai = c.gameObject.AddComponent<BasicAIInputControl>();
                ai.placing = c.input.placing;
                //
                Destroy(c.input);
                //
                c.input = ai;
            }
        }
    }

    internal bool isRaceDone = false;
    private void Game_ConcludeRace()
    {
        isRaceDone = true;

        raceWaypoints.Illuminate(-1);
    }






    /// <summary>
    /// Exists only to sort car placing.
    /// </summary>
    private class PlaceSorter : IComparable<PlaceSorter>
    {
        public Car car;
        public float placeValue;
        public PlaceSorter(Car c, float placeVal)
        {
            car = c;
            placeValue = placeVal;
        }

        public int CompareTo(PlaceSorter b)
        {
            PlaceSorter a = this;
            //
            if (a.car == b.car) { return 0; }
            else
            {
                if (a.placeValue < b.placeValue) { return 1; }
                else if (b.placeValue < a.placeValue) { return -1; }
                else { return 0; }
            }
        }
    }

    float placeCheckCounter;

    // Update is called once per frame
    void FixedUpdate () {
        if (!isRaceDone)
        {
            // *** Checks the car's placing every second.
            placeCheckCounter += Time.deltaTime;
            //
            if (placeCheckCounter >= 1)
            {
                placeCheckCounter -= 1;
                //
                GameManager gm = GameManager.Instance();
                List<PlaceSorter> carPlaces = new List<PlaceSorter>();
                for (int i = 0; i < allCars.Length; i++)
                {
                    Car c = allCars[i];
                    //
                    float val = 0;
                    // *** How many laps this car has done
                    val += 100 * (c.input.lapCount / 3f); // ??? <-- "gm.lapTotal"
                    // *** How many waypoints this car has passed
                    val += 10 * (1f * c.input.currentWaypoint / gm.waypoints.Length);

                    // *** How close the car is to the next waypoint, relative to the distance between it and the last waypoint
                    float gap = Vector3.Distance(gm.waypoints[c.input.currentWaypoint].position, gm.waypoints[(c.input.currentWaypoint + 1) % gm.waypoints.Length].position);
                    float dist = Vector3.Distance(c.transform.position, gm.waypoints[(c.input.currentWaypoint + 1) % gm.waypoints.Length].position);
                    val += 1 * Mathf.Max(0, 1f - dist / gap);
                    //
                    carPlaces.Add(new PlaceSorter(c, val));
                }
                //
                // *** The "Place Sorter" class has its own "CompareTo" function for this.
                carPlaces.Sort();
                //
                // *** Assign the new places
                for (int i = 0; i < carPlaces.Count; i++)
                {
                    PlaceSorter ps = carPlaces[i];
                    //
                    ps.car.input.placing = i;
                }
            }
        }
    }






    public static GameManager instance;
    public static GameManager Instance() { return instance; }
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

        Initialize();
    }
}
