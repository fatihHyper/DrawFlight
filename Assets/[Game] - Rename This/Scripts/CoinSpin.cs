using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpin : MonoBehaviour
{
    float speed = 20f;
    void Update()
    {
        transform.Rotate(Vector3.right * speed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        EventManager.OnCoinPickUp.Invoke();
        Destroy(gameObject);
        PoolingSystem.Instance.InstantiateAPS("CoinCollect", transform.position);

    }
}
