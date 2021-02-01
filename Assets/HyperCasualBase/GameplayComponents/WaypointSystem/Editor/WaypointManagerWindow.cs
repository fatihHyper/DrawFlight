using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

public class WaypointManagerWindow : OdinEditorWindow
{
    [MenuItem("Waypoint System/Waypoint Editor")]
    public static void Open()
    {
        GetWindow<WaypointManagerWindow>();
    }

    [SerializeField]
    [HideInInspector]
    private Transform WaypointRoot;

    protected override void OnGUI()
    {
        base.OnGUI();
        SerializedObject obj = new SerializedObject(this);

        if(WaypointRoot == null)
        {

            if (GUILayout.Button("SelectRoot"))
            {
                WaypointRoot = GameObject.Find("Waypoints").transform;
                if(WaypointRoot == null)
                {
                    CreateWaypointRoot();
                }
            }

        }
        else
        {
            GUILayout.BeginVertical();
            DrawButtons();
            GUILayout.EndVertical();
        }

        obj.ApplyModifiedProperties();
    }

    public void DrawButtons()
    {
        if (GUILayout.Button("Add Waypoint"))
            AddWaypoint();

        if (GUILayout.Button("Insert Waypoint"))
            InsertWaypoint();

        if (GUILayout.Button("Remove Waypoint"))
            RemoveWaypoint();

    }

    public void CreateWaypointRoot()
    {
        GameObject obj = new GameObject();
        obj.transform.position = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;
        obj.name = "Waypoints";
        WaypointRoot = obj.transform;
    }

    public void AddWaypoint()
    {
        if (WaypointRoot == null)
            CreateWaypointRoot();

        GameObject obj = new GameObject("Waypoint", typeof(Waypoint));
        obj.transform.SetParent(WaypointRoot, false);

        Waypoint waypoint = obj.GetComponent<Waypoint>();

        if(WaypointRoot.childCount > 1)
        {
            waypoint.PreviousWaypoint = WaypointRoot.GetChild(WaypointRoot.childCount - 2).GetComponent<Waypoint>();
            waypoint.PreviousWaypoint.NextWaypoint = waypoint;
            waypoint.transform.position = waypoint.PreviousWaypoint.transform.position;
            waypoint.transform.forward = waypoint.PreviousWaypoint.transform.forward;
        }

        Selection.activeGameObject = obj;
    }

    public void InsertWaypoint()
    {
        GameObject obj = new GameObject("Waypoint", typeof(Waypoint));
        obj.transform.SetParent(WaypointRoot);

        Waypoint newWaypoint = obj.GetComponent<Waypoint>();
        Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();

        obj.transform.position = selectedWaypoint.transform.position;
        obj.transform.forward = selectedWaypoint.transform.forward;
        newWaypoint.PreviousWaypoint = selectedWaypoint;

        if(selectedWaypoint.NextWaypoint != null)
        {
            selectedWaypoint.NextWaypoint.PreviousWaypoint = newWaypoint;
            newWaypoint.NextWaypoint = selectedWaypoint.NextWaypoint;
        }
        selectedWaypoint.NextWaypoint = newWaypoint;
        newWaypoint.transform.SetSiblingIndex(selectedWaypoint.transform.GetSiblingIndex()+1);
        Selection.activeGameObject = obj;
    }

    public void RemoveWaypoint()
    {
        Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();

        if (selectedWaypoint.NextWaypoint != null)
        {
            selectedWaypoint.NextWaypoint.PreviousWaypoint = selectedWaypoint.PreviousWaypoint;
        }
        if(selectedWaypoint.PreviousWaypoint != null)
        {
            selectedWaypoint.PreviousWaypoint.NextWaypoint = selectedWaypoint.NextWaypoint;
            Selection.activeGameObject = selectedWaypoint.PreviousWaypoint.gameObject;
        }

        DestroyImmediate(selectedWaypoint.gameObject);
    }
}
