using System.Collections.Generic;
using UnityEngine;

public class FollowAICarBrain : CarBrainBase
{
    System.Type interfaceType;

    public Waypoint currentWaypoint;

    private void OnEnable()
    {
        if (Managers.Instance == null) return;
        currentState = CarManager.Instance.CarAIStartState;
        remainState = CarManager.Instance.CarAIStartState;
        currentWaypoint = WaypointManager.Instance.GetClosestWaypoint(transform.position);
    }
    public override void Logic()
    {
        if (currentWaypoint == null || currentState == null) return;

        Vector3 waypointPos = currentWaypoint.GetPoisition();
        Motor.TargetPoint = waypointPos;

        if (Vector3.Distance(transform.position, new Vector3(waypointPos.x, transform.position.y, waypointPos.z)) <= Motor.CarControlData.CheckDistanceToWaypoint)
            currentWaypoint = currentWaypoint.NextWaypoint;

        //Debug.Log("Elapsed Frame Count :" + Time.frameCount);
        currentState.UpdateState(this);

       // Motor.SpringSystem(); test
        Motor.AnimateWheels();
    }

    public override void Initialize()
    {
        base.Initialize();
        currentWaypoint = WaypointManager.Instance.GetClosestWaypoint(transform.position);

    }
    public override void Dispose()
    {
        base.Dispose();
    }
    private void OnDrawGizmos()
    {

        if (currentState != null)
            Gizmos.color = currentState.sceneGizmoColor;

        if (currentWaypoint == null) return;
        //Gizmos.color = Color.black;

        //direction to waypoint
        Gizmos.DrawSphere(transform.position, 0.3f);
        Gizmos.DrawSphere(currentWaypoint.GetPoisition(), 0.3f);
        Gizmos.DrawLine(transform.position, currentWaypoint.GetPoisition());

    }

    public override void TransitionToState(State nextState)
    {
        if (currentState == nextState) return;
        currentState = nextState;
        OnExitState();

    }

    public void OnExitState()
    {

    }
}