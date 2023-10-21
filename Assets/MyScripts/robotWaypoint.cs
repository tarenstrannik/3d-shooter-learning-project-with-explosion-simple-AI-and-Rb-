using System;
using System.Collections.Generic;
using UnityEngine;

public class robotWaypoint : MonoBehaviour
{
    public static Array waypoints = null;
    public List<robotWaypoint> connected = null;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void Awake()
    {
        MakeWaypointList();
    }

    // Рисуем флажок
    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "Waypoint.tif");
    }

    void MakeWaypointList()
    {
        waypoints = FindObjectsByType<robotWaypoint>(FindObjectsSortMode.None);
        

        foreach ( robotWaypoint point  in waypoints)
        {
           
            point.AddWaypoints();
        }
    }

    void AddWaypoints()
    {
       connected.Clear();
        foreach (robotWaypoint wp in waypoints)
            if (wp != this) connected.Add(wp);
    }

    public static robotWaypoint FindNearestPoint(Vector3 pos )  {
        robotWaypoint nearestWP = null;
        float nearestDistance = float.MaxValue;
        foreach (robotWaypoint cur  in waypoints) {
            float distance = Vector3.Distance(cur.transform.position, pos);
		if (distance<nearestDistance )
		{
			nearestDistance  = distance;
			nearestWP = cur;
		}
	    }
	return nearestWP;
    }


}


