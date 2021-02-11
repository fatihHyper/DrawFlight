using UnityEngine;
using UnityEngine.AI;
using Dreamteck.Splines;
using System.Collections;

public class DrawManager : MonoBehaviour
{

    public GameObject M_rendererPrefab;
    public GameObject DrawPanel;
    public GameObject DrawObjPref;
    public Material Material;
    public Mesh Wallmesh;
    [HideInInspector] public GameObject transporter;

    [SerializeField] private float MaxLength = 10;
    [SerializeField] private Camera m_camera;
    private GameObject m_currentRenderer;
    private int layer_mask;
    private bool isDrawComeFromOutside;
    private GameObject createdDrawObj;
    public GameObject WingsPoint;
    private Vector3 closest;
    private float closestDist;
    private Vector3 startPos;
    private Vector3 lastPos;
    private int pointCount;
    private SplineComputer spline;
    private SplinePoint[] points;
    [HideInInspector] public float SplineLength;
    [HideInInspector] public string Direction;

    private void Start()
    {
        layer_mask = LayerMask.GetMask("DrawPanel");

        pointCount = 0;
    }

    private void Update()
    {

        RaycastHit hit;
        Ray _ray = m_camera.ScreenPointToRay(Input.touchCount == 1 ? (Vector3)Input.mousePosition : Input.mousePosition);



        if (IsInput(TouchPhase.Began) && LevelManager.Instance.IsLevelStarted)
        {
            if (Physics.Raycast(_ray, out hit, 1000f, layer_mask))
            {
                CreatSplineObject(hit);
                StartDrawing(hit);

                if (gameObject.transform.childCount != 0)
                {
                    for (int i = 0; i < gameObject.transform.childCount; i++)
                    {
                        Destroy(transform.GetChild(i).gameObject);
                    }
                }
                m_currentRenderer = (GameObject)Instantiate(M_rendererPrefab, hit.point, Quaternion.identity, transform);
            }




        }
        else if (IsInput(TouchPhase.Moved) && LevelManager.Instance.IsLevelStarted && !isDrawComeFromOutside)
        {
            if (Physics.Raycast(_ray, out hit, 1000f, layer_mask))
            {
                if (points != null)
                {
                    if (Vector3.Distance(lastPos, hit.point) >= 1f && pointCount < points.Length)
                    {
                        DrawWithMove(hit);

                    }
                    if (Mathf.Abs(DrawPanel.transform.position.x - hit.point.x) < closestDist)
                    {
                        closestDist = Mathf.Abs(DrawPanel.transform.position.x - hit.point.x);
                        closest = hit.point;
                    }
                    //2D draw with MouseMove
                    m_currentRenderer.transform.position = hit.point;
                    m_currentRenderer.layer = 12;
                }
                
            }
        }
        else if (IsInput(TouchPhase.Ended))
        {
            EndOfDraw();
        }

    }

    void CreatSplineObject(RaycastHit hit)
    {
        startPos = hit.point;
        lastPos = startPos;
        //Creation of base object for draw, Change layer for being sure it s not drawble on itself , Add Components
        createdDrawObj = (GameObject)Instantiate(DrawObjPref, startPos, Quaternion.identity);
        createdDrawObj.GetComponent<MeshRenderer>().material = Material;
        createdDrawObj.GetComponent<MeshRenderer>().enabled = false;
        spline = createdDrawObj.AddComponent<SplineComputer>();

        //SplineMesh Component adjust settings
        Dreamteck.Splines.SplineMesh splineMesh = createdDrawObj.AddComponent<Dreamteck.Splines.SplineMesh>();
        splineMesh.updateMethod = SplineUser.UpdateMethod.Update;
        spline.updateMode = SplineComputer.UpdateMode.AllUpdate;
        splineMesh.AddChannel(Wallmesh, "Wall");
        splineMesh.GetChannel(0).type = Dreamteck.Splines.SplineMesh.Channel.Type.Extrude;
        splineMesh.GetChannel(0).count = 50;
        splineMesh.GetChannel(0).minScale = new Vector3(1f, 0.3f, 0.1f);
        splineMesh.GetChannel(0).maxScale = new Vector3(1f, 0.3f, 0.1f);
        points = new SplinePoint[500];
        spline.type = Spline.Type.BSpline;
        spline.sampleMode = SplineComputer.SampleMode.Uniform;
        spline.customNormalInterpolation = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        spline.customValueInterpolation = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));


        closestDist = Mathf.Abs(DrawPanel.transform.position.x - hit.point.x);
        closest = hit.point;

    }
    void StartDrawing(RaycastHit hit)
    {
        //Set drawComef.  ,  Placing first draw point and set it  , increase pointCount
        isDrawComeFromOutside = false;
        points[pointCount] = new SplinePoint();
        points[pointCount].position = hit.point;
        points[pointCount].normal = Vector3.up;
        points[pointCount].size = 1f;
        points[pointCount].color = Color.white;

        spline.SetPoint(pointCount, points[pointCount], SplineComputer.Space.World);

        lastPos = hit.point;
        pointCount++;
        isDrawComeFromOutside = false;

    }

    void DrawWithMove(RaycastHit hit)
    {
        if (!isDrawComeFromOutside)
        {
            //Placing 3D draw points and mesh object creation
            points[pointCount] = new SplinePoint();
            points[pointCount].position = hit.point;
            points[pointCount].normal = Vector3.up;
            points[pointCount].size = 1f;
            points[pointCount].color = Color.white;

            spline.SetPoint(pointCount, points[pointCount], SplineComputer.Space.World);

            lastPos = hit.point;
            pointCount++;


        }
    }

    void EndOfDraw()
    {
        if (spline == null) return;
        var prevLength = SplineLength;
        SplineLength = spline.CalculateLength();
        if (SplineLength < 0.01) {
            SplineLength = prevLength;
            return; 
        } 
        double travel = spline.Travel(0, SplineLength / 2f, Spline.Direction.Forward);
        Vector3 middle = spline.EvaluatePosition(travel);

        if (middle.x > 1f)
        {
            Direction = "Right";
        }
        else if (middle.x < -1f)
        {
            Direction = "Left";
        }
        else
        {
            Direction = "Forward";
        }

        //Make disable all children of wingsPoint
        if (WingsPoint.transform.childCount != 0)
        {
            for (int i = 0; i < WingsPoint.transform.childCount; i++)
            {
                Destroy(WingsPoint.transform.GetChild(i).gameObject);
            }
        }

       
        Transform player = WingsPoint.GetComponentInParent<Transform>();
        

        GameObject transporter = (GameObject)Instantiate(DrawObjPref, closest, Quaternion.identity);

        if (createdDrawObj.GetComponent<MeshCollider>() == null)
        {
            createdDrawObj.AddComponent<MeshCollider>().convex = true;
        }
        else
        {

            createdDrawObj.GetComponent<MeshCollider>().convex = true;
        }

        createdDrawObj.transform.parent = transporter.transform;
        //createdDrawObj.transform.localPosition = Vector3.zero;
        
        transporter.transform.position = WingsPoint.transform.position;

        transporter.transform.rotation = WingsPoint.GetComponentInParent<Transform>().rotation;
        transporter.transform.parent = WingsPoint.transform;
        transporter.transform.localPosition = Vector3.zero;
        createdDrawObj.GetComponent<MeshRenderer>().enabled = true;
        isDrawComeFromOutside = true;


        Destroy(m_currentRenderer);

        EventManager.FirstDrawExist.Invoke();
        
    }


    private bool IsInput(TouchPhase phase)
    {
        switch (phase)
        {
            case TouchPhase.Began:
                return (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0);
            case TouchPhase.Moved:
                return (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) || Input.GetMouseButton(0);
            default:
                return (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) || Input.GetMouseButtonUp(0);
        }
    }

  


}


