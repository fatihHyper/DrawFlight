using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// This is an Base class derived from IAction to communicate with UI Panel and controlling game by Data from Panel.
/// </summary>
[RequireComponent(typeof(MultiChoiceData))]
public abstract class MCControllerBase : ActionBase
{
    public Choice Choice;

    public MultiChoiceData MultiChoiceData;

    [HideInInspector]
    public MultiChoicePanel MultiChoicePanel;
    [Button]
    public virtual void Trigger() {
        MultiChoicePanel = (MultiChoicePanel)UIIDHolder.Panels["MultiChoicePanel"];
        Begin(); 
        MultiChoicePanel.ShowPanel(this);
    }
}