using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class FlightController : MonoBehaviour
{
    [HideInInspector]
    public Vector3 direction;
    public GameObject CheckPointPref;
    public SplineComputer splineComputer;

    private Rigidbody _planeRigid;
    private float _speed;
    private bool _isFirstDraw;
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

    }

    private void OnDisable()
    {
        EventManager.FirstDrawExist.RemoveListener(() => _isFirstDraw = true);
    }


    private void Start()
    {

        points = splineComputer.GetPoints();
        _planeRigid = gameObject.GetComponent<Rigidbody>();
        _speed = 1200;
        checkPointNow = splineComputer.pointCount - 1;




    }

    private void FixedUpdate()
    {


        if (_isFirstDraw && LevelManager.Instance.IsLevelStarted)
        {

            rotationInfo = DrawManager.direction;


            if (DrawManager.splineLength < 1)
            {
                //transform.LookAt(points[checkPointNow].position - new Vector3(0, 100, 0));
                transform.Rotate( 0.5f * 100 * Time.deltaTime, 0, 0);
                //transform.rotation = Quaternion.FromToRotation(Vector3.up, rcHit.normal);
                // _planeRigid.velocity = transform.forward * _speed * 2 * Time.deltaTime;
            }
            else
            {
                GetRotation(rotationInfo);
                FollowTargetWithRotation(points[checkPointNow].position, 3, _speed);
               

            }

        }
    }

    void FollowTargetWithRotation(Vector3 target, float distanceToChange, float speed)
    {
        float distance = Vector3.Distance(new Vector3(target.x,0,target.z),new Vector3(transform.position.x,0,transform.position.z));
        Debug.Log(distance);
        Debug.Log(checkPointNow);
        if (distance > distanceToChange && checkPointNow > -1)
        {
            Vector3 targetDirection = (target - transform.position).normalized;
            targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 0.9f);

            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, localRotationZ);

            _planeRigid.velocity = transform.forward * speed *2  * Time.deltaTime;
        }
        else if (distance <= distanceToChange)
        {

            checkPointNow--;
            
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
                    localRotationZ++;
                }
                
                break;
            case "Right":
                if (localRotationZ > -40)
                {
                    localRotationZ--;
                }
                
                break;
            case "Forward":
                if (localRotationZ > 0)
                {
                    localRotationZ--;
                }
                else if (localRotationZ < 0)
                {
                    localRotationZ++;
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
