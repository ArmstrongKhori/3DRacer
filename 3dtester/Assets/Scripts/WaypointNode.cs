using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A single node in a list of waypoints.
/// It should have a "WaypointGizmo" as its parent or else it will likely break.
/// </summary>
public class WaypointNode : MonoBehaviour {

    public WaypointGizmos owner;
    public int wayID;
    internal SphereCollider sc;
    internal MeshFilter mf;
    internal MeshRenderer mr;


    /// <summary>
    /// Called by the "GameManager" upon its own creation.
    /// It MUST be done this way because Unity has very bad ordering of events...
    /// </summary>
    public void InitializeMe()
    {
        gameObject.layer = Helper.LAYER_WAYPOINT;
        gameObject.transform.localScale = new Vector3((25f) * 2, (25f) * 2, (25f) * 2);
        //
        //
        sc = gameObject.GetComponent<SphereCollider>();
        if (sc == null) { sc = gameObject.AddComponent<SphereCollider>(); }
        sc.radius = 0.5f;
        sc.isTrigger = true;
        //
        mf = gameObject.GetComponent<MeshFilter>();
        if (mf == null) { mf = gameObject.AddComponent<MeshFilter>(); }
        mf.mesh = GameManager.Instance()._sphere;
        //
        mr = gameObject.GetComponent<MeshRenderer>();
        if (mr == null) { mr = gameObject.AddComponent<MeshRenderer>(); }
        Debug.Log(mr);
        mr.material = (Material)Resources.Load("WaypointMat", typeof(Material));
        mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    }


    /// <summary>
    /// Tell any cars that pass that it just touched a waypoint node!
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        Car c = other.gameObject.GetComponent<Car>();
        if (c != null)
        {
            c.input.OnWaypointReached(c, this);
        }
    }
}
