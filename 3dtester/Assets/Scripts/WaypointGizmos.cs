using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Displays waypoints both in the game and the editor.
/// Needs to be populated with child objects-- Preferrably ones with the "WaypointNode" component.
/// </summary>
public class WaypointGizmos : MonoBehaviour {

    public float size = 1f;
    public Transform[] waypoints;
    public int Count { get { return waypoints.Length - 1; } }



    // Use this for initialization
    void Start () {
        waypoints = gameObject.GetComponentsInChildren<Transform>();
        //
        for (int i = 1; i < waypoints.Length; i++)
        {
            WaypointNode wn = waypoints[i].gameObject.GetComponent<WaypointNode>();
            wn.owner = this;
            wn.wayID = i - 1;
            wn.transform.LookAt(waypoints[(i + 1) % waypoints.Length]);
        }
        //
        //
        Illuminate(0);
    }


    public void Illuminate(int thisOne, bool turnOffAll = true)
    {
        for (int i = 1; i < waypoints.Length; i++)
        {
            WaypointNode wn = waypoints[i].gameObject.GetComponent<WaypointNode>();
            MeshRenderer mr = waypoints[i].gameObject.GetComponent<MeshRenderer>();
            if (wn.wayID == thisOne) { mr.enabled = true; }
            else
            {
                if (turnOffAll) { mr.enabled = false; }
            }
            
        }
    }



    private void OnDrawGizmos()
    {
        waypoints = gameObject.GetComponentsInChildren<Transform>();

        Vector3 last = waypoints[waypoints.Length - 1].position;
        for (int i = 1; i < waypoints.Length; i++)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(waypoints[i].position, size);
            Gizmos.DrawLine(last, waypoints[i].position);
            last = waypoints[i].position;
            //
            Gizmos.color = new Color(0.7f, 0.7f, 0.7f, 0.3f);
            Gizmos.DrawSphere(waypoints[i].position, 25f);
        }
    }
}
