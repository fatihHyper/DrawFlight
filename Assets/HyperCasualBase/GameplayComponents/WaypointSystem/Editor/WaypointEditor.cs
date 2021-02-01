using UnityEditor;
using UnityEngine;

[InitializeOnLoad()]
public class WaypointEditor 
{
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
    public static void OnDrawSceneGizmos(Waypoint waypoint, GizmoType gizmoType)
    {
        if((gizmoType & GizmoType.Selected) != 0)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.yellow * 0.5f;
        }

        Gizmos.DrawSphere(waypoint.transform.position, 0.1f);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(waypoint.transform.position + (waypoint.transform.right * waypoint.Widht / 2f),
            waypoint.transform.position - (waypoint.transform.right * waypoint.Widht / 2f));

        if(waypoint.PreviousWaypoint != null)
        {
            Gizmos.color = Color.red;
            Vector3 offset = waypoint.transform.right * waypoint.Widht / 2f;
            Vector3 offsetTo = waypoint.PreviousWaypoint.transform.right * waypoint.PreviousWaypoint.Widht / 2f;
            Gizmos.DrawLine(waypoint.transform.position + offset, waypoint.PreviousWaypoint.transform.position + offsetTo);
        }

        if(waypoint.NextWaypoint != null)
        {
            Gizmos.color = Color.green;
            Vector3 offset = waypoint.transform.right * -waypoint.Widht / 2f;
            Vector3 offsetTo = waypoint.NextWaypoint.transform.right * -waypoint.NextWaypoint.Widht / 2f;
            Gizmos.DrawLine(waypoint.transform.position + offset, waypoint.NextWaypoint.transform.position + offsetTo);
        }
    }
}
