using UnityEngine;


public abstract class CarBrainBase : InterfaceBase, ICarBrain
{
    public MonoBehaviour MonoBehaviour { get { return this; } }

    System.Type interfaceType;

    public State currentState;
    public State remainState;


    private IMotor motor;
    public IMotor Motor { get { return Utilities.IsNullOrDestroyed(motor, out interfaceType) ? motor = GetComponent<IMotor>() : motor; } }
    public abstract void Logic();

    public virtual void Initialize()
    {
        Debug.Log("Car Brain Intialize" + GetType().ToString());
    }

    public virtual void Dispose()
    {
        Debug.Log("Car Brain Disposed" + GetType().ToString());
        Utilities.DestroyExtended(this);
    }

    public abstract void TransitionToState(State state);
    
      
    
}
