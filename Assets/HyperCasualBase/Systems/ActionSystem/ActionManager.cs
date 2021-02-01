using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;


public class ActionManager : Singleton<ActionManager>
{

    public List<IAction> actions;
    public List<IAction> Actions { get { return (actions == null) ? actions = new List<IAction>() : actions; } set { actions = value; } }

    public UnityEvent OnActionsComplete = new UnityEvent();

    public IAction CurrentAction { get; set; }

    private void OnEnable()
    {
        if (Managers.Instance == null)
            return;
    }

    private void OnDisable()
    {
        if (Managers.Instance == null)
            return;
    }


    public void AddAction(IAction action)
    {
        if (!Actions.Contains(action))
            Actions.Add(action);
    }

    public void RemoveAction(IAction action)
    {
        if (Actions.Contains(action))
            Actions.Remove(action);
    }

    public void CompleteAction(IAction action)
    {
        if (ReferenceEquals(CurrentAction, action))
            CurrentAction = null;

        RemoveAction(action);
        CheckActionList();
    }

    public void CheckActionList()
    {
        if(Actions.Count == 0)
        {
            OnActionsComplete.Invoke();
            GameManager.Instance.CompilateStage(true);
        }
    }

    public void CompleteAllActions()
    {
        List<IAction> actions = new List<IAction>(Actions);
        foreach (var action in actions)
        {
            action.Complete();
        }
    }
}
