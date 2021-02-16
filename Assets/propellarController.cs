using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class propellarController : MonoBehaviour
{
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        transform.rotation *= Quaternion.AngleAxis(20, Vector3.forward);
    }
}
