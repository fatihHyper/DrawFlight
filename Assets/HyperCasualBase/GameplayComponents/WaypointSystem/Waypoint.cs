using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Waypoint : MonoBehaviour
{
    public Waypoint PreviousWaypoint;
    public Waypoint NextWaypoint;

    [Range(0, 5)]
    public float Widht = 0;


    private void OnEnable()
    {
        if (Managers.Instance == null) return;
        SceneController.Instance.OnSceneLoaded.AddListener(() => WaypointManager.Instance.AddWaypoint(this));
        LevelManager.Instance.OnLevelFinish.AddListener(() => WaypointManager.Instance.RemovewayPoint(this));
    }

    private void OnDisable()
    {
        if (Managers.Instance == null) return;
        SceneController.Instance.OnSceneLoaded.AddListener(() => WaypointManager.Instance.AddWaypoint(this));
        LevelManager.Instance.OnLevelFinish.AddListener(() => WaypointManager.Instance.RemovewayPoint(this));
    }

    public Vector3 GetPoisition()
    {
        Vector3 minBound = transform.position + transform.right * Widht / 2;
        Vector3 maxBound = transform.position - transform.right * Widht / 2;

        return Vector3.Lerp(minBound, maxBound, Random.Range(0f, 1f));
    }
}
