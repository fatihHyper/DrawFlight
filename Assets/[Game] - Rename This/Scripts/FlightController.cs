using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class FlightController : MonoBehaviour
{
    private Rigidbody _planeRigid;
    private float _speed;
    private float _rotationSpeed;
    [HideInInspector]
    public bool _isTurn;
    private bool _isFirstDraw;
    private string rotationInfo;
    [HideInInspector]
    public Vector3 direction;


    private static DrawManager drawManager;
    private static DrawManager DrawManager { get { return (drawManager == null) ? drawManager = FindObjectOfType<DrawManager>() : drawManager; } set { drawManager = value; } }

    private bool isRightWeight;

    private Transform FlightTransform;
    private SplineFollower splineFollower;
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
        //splineFollower = gameObject.GetComponent<SplineFollower>();
        _planeRigid = gameObject.GetComponent<Rigidbody>();
        _speed = 800f;
        _rotationSpeed = 80f;
        rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        //_isTurn = false;
       
    }

    private void FixedUpdate()
    {
        

        if (_isFirstDraw && LevelManager.Instance.IsLevelStarted)
        {
            rotationInfo = DrawManager.direction;

                
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
                        _planeRigid.MovePosition(new Vector3(transform.position.x, 20, transform.position.z));
                        GetRotation(rotationInfo);
                        transform.localRotation =  Quaternion.Slerp(transform.localRotation, rotation, Time.deltaTime );
                    
                    }
                
                }

            direction = transform.TransformDirection(Vector3.forward);
            _planeRigid.velocity = direction * _speed * 2 * Time.deltaTime;
            
            
            

        }
    }

    void GetRotation(string rotationInfo)
    {
            switch (rotationInfo)
            {

                case "Left":

                    rotation = Quaternion.Euler(Vector3.forward * 20);

                    break;
                case "Right":
                    rotation = Quaternion.Euler(Vector3.forward * -20);
                    break;
                case "Forward":
                    rotation = Quaternion.Euler(Vector3.forward);
                    break;

                default:
                    break;
            }
        
            
        
        
    }
   
}
