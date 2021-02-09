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

    public GameObject CheckPointPref;


    private SplinePoint[] points;
    private int checkPointNow;
    private Vector3 rotation;
    private Quaternion targetRotation;
    private int localRotationZ;
    private int localRotationX;

    private static DrawManager drawManager;
    private static DrawManager DrawManager { get { return (drawManager == null) ? drawManager = FindObjectOfType<DrawManager>() : drawManager; } set { drawManager = value; } }

    public SplineComputer splineComputer;

    private void OnEnable()
    {

        EventManager.FirstDrawExist.AddListener(() => _isFirstDraw = true);
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
        _speed = 1200;
        _rotationSpeed = 80f;
        rotation = new Vector3(0, 0, 0);
        checkPointNow = splineComputer.pointCount - 1;

        for (int i = 0; i < points.Length; i++)
        {

            Instantiate(CheckPointPref, points[i].position, Quaternion.identity);
        }



    }

    private void FixedUpdate()
    {

        Debug.Log(checkPointNow);

        if (_isFirstDraw && LevelManager.Instance.IsLevelStarted)
        {

            rotationInfo = DrawManager.direction;


            if (DrawManager.splineLength < 1)
            {
                //transform.LookAt(points[checkPointNow].position - new Vector3(0, 100, 0));
                transform.Rotate( 0.5f * 100 * Time.deltaTime, 0, 0);
                _planeRigid.velocity = transform.forward * _speed * 2 * Time.deltaTime;
            }
            else
            {
                GetRotation(rotationInfo);
                FollowTargetWithRotation(points[checkPointNow].position, 0f, _speed);
               

            }

        }
    }

    void FollowTargetWithRotation(Vector3 target, float distanceToStop, float speed)
    {
        if (Vector3.Distance(transform.position, target) > distanceToStop && checkPointNow > -1)
        {
            Vector3 targetDirection = (target - transform.position).normalized;
            targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 0.6f );

            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, localRotationZ);

            _planeRigid.velocity = transform.forward * speed *2  * Time.deltaTime;
        }
        else if (Vector3.Distance(transform.position, points[checkPointNow].position) <= 0.5f && checkPointNow <= -1)
        {
            transform.LookAt(Vector3.forward);
            _planeRigid.isKinematic = true;
            
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
