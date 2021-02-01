using UnityEngine;
using Sirenix.OdinInspector;
using System;
using UnityEditor.Presets;

[Serializable]
public struct CarControlData
{
    public CarControlType CarControlType;
    [ShowIf("isAI")] public CarAIType CarAIType;
    [ShowIf("isAI")] public float CheckDistanceToWaypoint;
    [ShowIf("isAI")] public float MaxSpeed;

    public float MotorTorque;
    public float BrakeTorque;
    public float MaxSteerAngle;
    public float TurnSensitivity;

    private bool isAI { get { return CarControlType == CarControlType.AI; } }
}

[Serializable]
public struct CarGraphicData
{
    public GameObject BodyPrefab;
    public GameObject FrontWheelPrefab;
    public GameObject RearWheelPrefab;

    private Mesh bodyMesh;
    public Mesh BodyMesh
    {
        get
        {
            if (BodyPrefab != null)
                return (bodyMesh == null) ? bodyMesh = BodyPrefab.GetComponentInChildren<MeshFilter>().sharedMesh : bodyMesh;
            return null;
        }
    }
}


[Serializable]
public struct WheelColliderData
{
    [InlineEditor]
    public Preset WheelColliderPreset;
}
public enum CarControlType { None, Player, AI }
public enum CarAIType { FollowWaypoint }
public class CarData : ScriptableObject
{
    [BoxGroup("Graphics")]
    public CarGraphicData CarGraphicData;
    [BoxGroup("Control")]
    public CarControlData CarControlData;
    [BoxGroup("WheelData")]
    public WheelColliderData WheelColliderData;
}
