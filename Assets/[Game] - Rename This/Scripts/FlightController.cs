using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightController : MonoBehaviour
{
    private Rigidbody _planeRigid;
    private float _speed;
    private float _rotationSpeed;

    private bool _isFirstDeparture;
    private bool _isFirstDraw;
    private string direction;

    private static DrawManager drawManager;
    private static DrawManager DrawManager { get { return (drawManager == null) ? drawManager = FindObjectOfType<DrawManager>() : drawManager; } set { drawManager = value; } }

    private bool isRightWeight;

    private Transform FlightTransform;

    private Quaternion rotation;
    private void OnEnable()
    {
       
        EventManager.FirstDrawExist.AddListener(() => _isFirstDraw = true );

    }

    private void OnDisable()
    {
        EventManager.FirstDrawExist.RemoveListener(() => _isFirstDraw = true);
    }


    private void Start()
    {
        _planeRigid = gameObject.GetComponent<Rigidbody>();
        _speed = 800f;
        _rotationSpeed = 60f;
        _isFirstDeparture = true;
        rotation = Quaternion.Euler(new Vector3(0, 0, 0));

    }

    private void FixedUpdate()
    {
        
        if (_isFirstDraw && LevelManager.Instance.IsLevelStarted)
        {
            direction = DrawManager.direction;


                if (gameObject.transform.position.y < 20)
                {
                    if (DrawManager.splineLength > 1)
                    {
                        transform.Rotate(-0.05f * _rotationSpeed * 2 * Time.deltaTime, 0, 0);
                    }
                    else
                    {
                        transform.Rotate(0.05f * _rotationSpeed * 2 *  Time.deltaTime, 0, 0);

                    }

                }
                else if (gameObject.transform.position.y >= 20)
                {
                    if (DrawManager.splineLength < 1)
                    {
                        transform.Rotate(0.05f * _rotationSpeed * 2  * Time.deltaTime, 0, 0);
                    }
                    else
                    {
                        
                        _planeRigid.MovePosition(new Vector3(gameObject.transform.position.x, 20, transform.position.z));
                        GetRotation(direction);
                        transform.localRotation = Quaternion.Slerp(transform.localRotation, rotation, Time.deltaTime * 1f);
                       
                    }
                
            }
            _planeRigid.velocity = transform.forward * _speed * 2 * Time.deltaTime;

        }
    }

    void GetRotation(string direction)
    {
        Debug.Log(direction);
        switch (direction)
        {

            case "Left":
                rotation = Quaternion.Euler(Vector3.forward * 20);

                break;
            case "Right":
                rotation = Quaternion.Euler(Vector3.forward * -20);
                break;
            case "Forward":
                rotation = Quaternion.Euler(Vector3.zero);
                break;
            
            default:
                break;
        }
    }
   
}
