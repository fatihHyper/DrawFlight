using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : InterfaceBase, ICameraTarget
{
    private void OnEnable()
    {
        SubToCamera();
    }

    private void OnDisable()
    {
        UnSubToCamera();
    }

    public void SubToCamera()
    {
        if (CameraController.Instance == null)
            return;

        CameraController.Instance.AddTarget(this);
    }

    public void UnSubToCamera()
    {
        if (CameraController.Instance == null)
            return;

        CameraController.Instance.RemoveTarget(this);
    }
}
