using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class FlightController : MonoBehaviour
{
    [HideInInspector]
    public Vector3 direction;
    [SerializeField]
    private SplineComputer splineComputer;

    private Rigidbody _planeRigid;
    private float _speed;
    private bool _isFirstDraw;
    private bool _isCrush;
    private string rotationInfo;
    private SplinePoint[] points;
    private int checkPointNow;
    private Quaternion targetRotation;
    private int localRotationZ;

    private static DrawManager drawManager;
    private static DrawManager DrawManager { get { return (drawManager == null) ? drawManager = FindObjectOfType<DrawManager>() : drawManager; } set { drawManager = value; } }


    private void OnEnable()
    {

        EventManager.FirstDrawExist.AddListener(() => _isFirstDraw = true);
        EventManager.FlightCrash.AddListener(() => _isCrush = true);

    }

    private void OnDisable()
    {
        EventManager.FirstDrawExist.RemoveListener(() => _isFirstDraw = true);
        EventManager.FlightCrash.RemoveListener(() => _isCrush = true);
    }


    private void Start()
    {

        points = splineComputer.GetPoints();
        _planeRigid = gameObject.GetComponent<Rigidbody>();
        _speed = 1100;
        checkPointNow = 0;
    }

    private void FixedUpdate()
    {
        if (_isCrush)
        {
            _planeRigid.velocity = Vector3.zero;
            _planeRigid.angularVelocity = Vector3.zero;
        }

        else
        {
            if (_isFirstDraw)
            {

                rotationInfo = DrawManager.Direction;


                if (DrawManager.SplineLength <6 && _planeRigid.velocity != Vector3.zero)
                {
                    transform.Rotate(0.5f * 100 * Time.fixedDeltaTime, 0, 0);
                    _planeRigid.velocity = transform.forward * _speed * 2 * Time.deltaTime;
                }
                else
                {
                    GetRotation(rotationInfo);
                    FollowTargetWithRotation(points[checkPointNow].position, 6, _speed);


                }

            }
        }
       
    }

    void FollowTargetWithRotation(Vector3 target, float distanceToChange, float speed)
    {
        float distanceX = Vector3.Distance(new Vector3(target.x,0,0),new Vector3(transform.position.x,0,0));  
        float distanceZ = Vector3.Distance(new Vector3(0,0,target.z),new Vector3(0,0,transform.position.z));  
       
        if ((distanceX > distanceToChange || distanceZ > distanceToChange) && checkPointNow < splineComputer.pointCount && target != transform.position)
        {
            Vector3 targetDirection = (target - transform.position).normalized;
            if (_planeRigid.velocity != Vector3.zero && targetDirection != Vector3.zero)
            {   
                targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,Time.fixedDeltaTime);

                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, localRotationZ);
            }
          

            _planeRigid.velocity = transform.forward * speed * Time.deltaTime;
        }
        else if ((distanceX <= distanceToChange || distanceZ <= distanceToChange))
        {

            checkPointNow++;
            
        }

    }


    void GetRotation(string rotationInfo)
    {
        
        //Get rotation info and rotate plane smoothly
        switch (rotationInfo)
        {
            case "Left":
                if (localRotationZ <40)
                {
                    localRotationZ += 2;
                }
                
                break;
            case "Right":
                if (localRotationZ > -40)
                {
                    localRotationZ -= 2;
                }
                
                break;
            case "Forward":
                if (localRotationZ > 0)
                {
                    localRotationZ -= 2;
                }
                else if (localRotationZ < 0)
                {
                    localRotationZ+=2;
                }
                else
                {

                    localRotationZ = 0;
                }


                break;

            default:
                break;
        }




    }

}
