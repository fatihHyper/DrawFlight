using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class WaypointManager : Singleton<WaypointManager>
{
    [ReadOnly]
    public List<Waypoint> Waypoints = new List<Waypoint>();

    private Waypoint lastWaypoint;
    public Waypoint LastWaypoint
    {
        get
        {
            if (lastWaypoint == null)
            {
                foreach (var wp in Waypoints)
                {
                    if (wp.NextWaypoint == null)
                    {
                        lastWaypoint = wp;
                        return wp;
                    }
                }
            }
            return lastWaypoint;
        }
    }

    public void AddWaypoint(Waypoint waypoint)
    {
        if (!Waypoints.Contains(waypoint))
            Waypoints.Add(waypoint);
    }

    public void RemovewayPoint(Waypoint waypoint)
    {
        if (Waypoints.Contains(waypoint))
            Waypoints.Remove(waypoint);
    }

    public Waypoint GetClosestWaypoint(Vector3 position)
    {
        float minDist = Mathf.Infinity;
        Waypoint closestWaypoint = null;

        for (int i = 0; i < Waypoints.Count; i++)
        {
            float dist = Vector3.Distance(position, Waypoints[i].transform.position);
            if(dist < minDist)
            {
                closestWaypoint = Waypoints[i];
                minDist = dist;
            }
        }

        return closestWaypoint;
    }
}
