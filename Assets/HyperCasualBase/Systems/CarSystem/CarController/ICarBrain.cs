using UnityEngine;

public interface ICarBrain : IComponent
{
    MonoBehaviour MonoBehaviour { get; }
    void Initialize();
    void Logic();
    void Dispose();
}

