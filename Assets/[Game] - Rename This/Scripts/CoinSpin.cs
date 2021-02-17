using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpin : MonoBehaviour
{
    float speed = 50f;
    void Update()
    {
        transform.Rotate(Vector3.up * speed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        EventManager.OnCoinPickUp.Invoke();
        PoolingSystem.Instance.InstantiateAPS("CoinCollect", transform.position);
        Destroy(gameObject);
        

    }
}
