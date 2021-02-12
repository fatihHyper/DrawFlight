using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashController : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag != "StartPlatform" && collision.transform.tag != "WingPoint" && collision.transform.tag != "Wing")
        {
            StartCoroutine(Wait());
           
        }
       
    }
    IEnumerator Wait()
    {
        EventManager.FlightCrash.Invoke();

        PoolingSystem.Instance.InstantiateAPS("FlightFireExplosion", gameObject.transform.position);
       
        //gameObject.GetComponent<MeshRenderer>().enabled = false;
        //gameObject.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(1f);
        GameManager.Instance.CompilateStage(false);
        

    }



}
