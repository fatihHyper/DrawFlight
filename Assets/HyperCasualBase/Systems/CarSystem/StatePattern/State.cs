using UnityEngine;

public class State : ScriptableObject
{
    public ActionBehaviour[] actionBehaviours;
    public Transition[] transitions;
    public Color sceneGizmoColor = Color.grey;

    public void UpdateState(CarBrainBase carBrainBase)
    {
        DoActions(carBrainBase);
        CheckTransitions(carBrainBase);
    }

    private void DoActions(CarBrainBase carBrainBase)
    {
        for (int i = 0; i < actionBehaviours.Length; i++)
        {
            actionBehaviours[i].Act(carBrainBase);
        }
    }

    private void CheckTransitions(CarBrainBase carBrainBase)
    {
        for (int i = 0; i < transitions.Length; i++)
        {
            bool decisionSucceeded = transitions[i].decision.Decide(carBrainBase);

            if (decisionSucceeded)
            {
                carBrainBase.TransitionToState(transitions[i].trueState);
                break;
            }
            else
            {
                carBrainBase.TransitionToState(transitions[i].falseState);
            }
        }
    }

}