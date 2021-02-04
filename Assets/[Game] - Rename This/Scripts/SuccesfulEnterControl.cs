using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccesfulEnterControl : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PoolingSystem.Instance.InstantiateAPS("Confetti", gameObject.transform.position);
    }
}
