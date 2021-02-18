using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("player entered");
            GameManager.Instance.CompilateStage(true);
            DrawManager.Instance.enabled = false;
            HapticManager.Haptic(HapticTypes.SoftImpact);
        }
        

    }
}
