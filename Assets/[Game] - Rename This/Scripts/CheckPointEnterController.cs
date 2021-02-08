using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointEnterController : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            EventManager.NextCheckPoint.Invoke();
            gameObject.SetActive(false);
        }
       
    }
}
