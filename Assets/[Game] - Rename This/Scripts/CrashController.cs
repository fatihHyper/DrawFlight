using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashController : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        PoolingSystem.Instance.InstantiateAPS("FlightFireExplosion", collision.gameObject.transform.position);
        Destroy(collision.gameObject.GetComponent<MeshRenderer>());
        GameManager.Instance.CompilateStage(false);

    }
}
