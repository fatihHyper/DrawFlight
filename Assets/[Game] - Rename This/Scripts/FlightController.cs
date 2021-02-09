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
   
    public SplinePoint[] points;
    public int checkPointNow;
    private Vector3 rotation;

    private static DrawManager drawManager;
    private static DrawManager DrawManager { get { return (drawManager == null) ? drawManager = FindObjectOfType<DrawManager>() : drawManager; } set { drawManager = value; } }

    public SplineComputer splineComputer;
    
    private void OnEnable()
    {
       
        EventManager.FirstDrawExist.AddListener(() => _isFirstDraw = true );
        EventManager.NextCheckPoint.AddListener(() => checkPointNow--);

    }

    private void OnDisable()
    {
        EventManager.FirstDrawExist.RemoveListener(() => _isFirstDraw = true);
        EventManager.NextCheckPoint.RemoveListener(() => checkPointNow--);
    }


    private void Start()
    {

        points = splineComputer.GetPoints();
        _planeRigid = gameObject.GetComponent<Rigidbody>();
        _speed = 1200f;
        _rotationSpeed = 80f;
        rotation = new Vector3(0, 0, 0);
        checkPointNow = splineComputer.pointCount-1;
    }

    private void FixedUpdate()
    {

        Debug.Log(checkPointNow);
        if (_isFirstDraw && LevelManager.Instance.IsLevelStarted)
        {
            rotationInfo = DrawManager.direction;

                
                    if (DrawManager.splineLength < 1)
                    {
                        transform.LookAt(points[checkPointNow].position - new Vector3(0,60,0));
                        if (rotation.x < 45)
                        {
                            rotation += new Vector3(1, 0, 0);
                        }
                        else
                        {
                            rotation = new Vector3(45, 0, 0);
                        }
                        _planeRigid.velocity = transform.forward * _speed * Time.deltaTime;
                    }
                    else
                    {

                        FollowTargetWithRotation(points[checkPointNow].position, 0f, _speed);
                        GetRotation(rotationInfo);
                    
                    }
                
        }
    }

    void FollowTargetWithRotation(Vector3 target, float distanceToStop, float speed)
    {
        if (Vector3.Distance(transform.position, target) > distanceToStop)
        {
            transform.LookAt(target);
            transform.rotation *= Quaternion.Euler(rotation);
            _planeRigid.velocity = transform.forward * speed * Time.deltaTime;
        }
    }


    void GetRotation(string rotationInfo)
    {
            switch (rotationInfo)
            {

                case "Left":

                if (rotation.z < 45)
                {
                    rotation += new Vector3(0, 0, 1);
                }
                else
                {
                    rotation = new Vector3(0, 0, 45);
                }
                    
                    break;
                case "Right":
                if (rotation.z > -45)
                {
                    rotation -= new Vector3(0, 0, 1);
                }
                else
                {
                    rotation = new Vector3(0, 0, -45);
                }
                break;
                case "Forward":
                if (rotation.z > 0)
                {
                    rotation -= new Vector3(0, 0, 1);
                }
                else if (rotation.z < 0)
                {
                    rotation += new Vector3(0, 0, 1);
                }
                else
                {
                    rotation = new Vector3(0, 0, 0);
                }
                
                    break;

                default:
                    break;
            }
        
            
        
        
    }
   
}
