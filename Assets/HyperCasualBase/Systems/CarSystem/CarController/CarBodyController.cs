using System.Collections.Generic;
using UnityEngine;

public class CarBodyController : MonoBehaviour
{
    public WheelHolder FrontWheelHolder;

    public List<WheelHolder> RearWheelHolders;

    public GasParticleHolder GasParticleHolder;
    

    private void OnEnable()
    {
        if (Managers.Instance == null) return;
    }

    private void OnDisable()
    {
        if (Managers.Instance == null) return;
    }


}
