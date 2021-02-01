using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public abstract class ActionBase : InterfaceBase, IAction
{
    public bool isComplate { get; set; }

    protected virtual void OnEnable()
    {
        if (Managers.Instance == null)
            return;

        ActionManager.Instance.AddAction(this);
    }

    protected virtual void OnDisable()
    {
        if (Managers.Instance == null)
            return;

        ActionManager.Instance.RemoveAction(this);
    }

    public abstract void Begin();

    public abstract void Do();

    public virtual void Complete()
    {
        isComplate = true;
        if(ActionManager.Instance != null)
            ActionManager.Instance.CompleteAction(this);
    }

    private void OnDestroy()
    {
        if (Managers.Instance == null)
            return;

        ActionManager.Instance.RemoveAction(this);
    }
}
