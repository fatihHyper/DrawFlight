using UnityEngine;
using UnityEngine.AI;
using Dreamteck.Splines;

public class DrawManager : MonoBehaviour
{

    public GameObject m_rendererPrefab;
    private GameObject m_currentRenderer;


    public GameObject transformPanel;
    private GameObject wingsPoint;

    private int layer_mask;
    private bool isDrawComeFromOutside;
    public GameObject drawObjPref;
    private GameObject createdDrawObj;
    public Material material;
    public Mesh Wallmesh;
    [HideInInspector]
    public GameObject transporter;


    private Vector3 closest;
    private float closestDist;

    private Vector3 startPos;
    private Vector3 lastPos;
    [SerializeField] private Camera m_camera;

    private int pointCount;
    private SplineComputer spline;
    private SplinePoint[] points;
    [HideInInspector]
    public string direction;
    [HideInInspector]
    public float _leftSideWeight;
    [HideInInspector]
    public float _rightSideWeight;
    [HideInInspector]
    public Vector3 middle;
    [HideInInspector]
    public float splineLength;




    private void Start()
    {
        layer_mask = LayerMask.GetMask("DrawPanel");

        pointCount = 0;

    }

    private void FixedUpdate()
    {

        RaycastHit hit;
        Ray _ray = m_camera.ScreenPointToRay(Input.touchCount == 1 ? (Vector3)Input.mousePosition : Input.mousePosition);

        if (Physics.Raycast(_ray, out hit, 1000f, layer_mask))
        {
            if (IsInput(TouchPhase.Began) && LevelManager.Instance.IsLevelStarted)
            {


                CreatSplineObject(hit);
                StartDrawing(hit);
                wingsPoint = GameObject.FindWithTag("WingPoint");

                m_currentRenderer = (GameObject)Instantiate(m_rendererPrefab, hit.point, Quaternion.identity, wingsPoint.transform);


            }
            else if (IsInput(TouchPhase.Moved) && LevelManager.Instance.IsLevelStarted && !isDrawComeFromOutside)
            {

                if (Vector3.Distance(lastPos, hit.point) >= 1f && pointCount < points.Length)
                {
                    DrawWithMove(hit);

                }
                if (Mathf.Abs(transformPanel.transform.position.x - hit.point.x) < closestDist)
                {
                    closestDist = Mathf.Abs(transformPanel.transform.position.x - hit.point.x);
                    closest = hit.point;
                }
                //2D draw with MouseMove
                m_currentRenderer.transform.position = hit.point;
                m_currentRenderer.layer = 12;



            }
            else if (IsInput(TouchPhase.Ended))
            {
                if (Vector3.Distance(m_currentRenderer.transform.position, hit.point) < 0.1f)
                    Destroy(m_currentRenderer);

                EndOfDraw();
            }
        }

    }

    void CreatSplineObject(RaycastHit hit)
    {
        _leftSideWeight = 0;
        _rightSideWeight = 0;

        startPos = hit.point;
        lastPos = startPos;
        //Creation of base object for draw, Change layer for being sure it s not drawble on itself , Add Components
        createdDrawObj = (GameObject)Instantiate(drawObjPref, startPos, Quaternion.identity, transformPanel.transform);
        createdDrawObj.layer = LayerMask.NameToLayer("Ignore Raycast");
        createdDrawObj.GetComponent<MeshRenderer>().material = material;
        createdDrawObj.GetComponent<MeshRenderer>().enabled = false;
        spline = createdDrawObj.AddComponent<SplineComputer>();

        //SplineMesh Component adjust settings
        Dreamteck.Splines.SplineMesh splineMesh = createdDrawObj.AddComponent<Dreamteck.Splines.SplineMesh>();
        splineMesh.updateMethod = SplineUser.UpdateMethod.FixedUpdate;
        spline.updateMode = SplineComputer.UpdateMode.FixedUpdate;
        splineMesh.AddChannel(Wallmesh, "Wall");
        splineMesh.GetChannel(0).type = Dreamteck.Splines.SplineMesh.Channel.Type.Extrude;
        splineMesh.GetChannel(0).count = 50;
        splineMesh.GetChannel(0).minScale = new Vector3(1f, 0.3f, 0.1f);
        splineMesh.GetChannel(0).maxScale = new Vector3(1f, 0.3f, 0.1f);
        points = new SplinePoint[100];
        spline.type = Spline.Type.BSpline;
        spline.sampleMode = SplineComputer.SampleMode.Uniform;
        spline.customNormalInterpolation = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        spline.customValueInterpolation = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));


        closestDist = Mathf.Abs(transformPanel.transform.position.x - hit.point.x);
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

        splineLength = spline.CalculateLength();
        double travel = spline.Travel(0, splineLength / 2f, Spline.Direction.Forward);
        Vector3 middle = spline.EvaluatePosition(travel);


        if (middle.x > 0.5f)
        {
            direction = "Right";
        }
        else if (middle.x < -0.5f)
        {
            direction = "Left";
        }
        else
        {
            direction = "Forward";
        }

        //Make disable all children of wingsPoint
        wingsPoint = GameObject.FindWithTag("WingPoint");
        if (wingsPoint.transform.childCount != 0)
        {
            for (int i = 0; i < wingsPoint.transform.childCount; i++)
            {
                Destroy(wingsPoint.transform.GetChild(i).gameObject);
            }
        }

        GameObject transporter = (GameObject)Instantiate(drawObjPref, closest, Quaternion.identity);
        createdDrawObj.AddComponent<MeshCollider>().convex = true;
        createdDrawObj.transform.parent = transporter.transform;
        transporter.transform.position = wingsPoint.transform.position;

        transporter.transform.rotation = wingsPoint.GetComponentInParent<Transform>().rotation;
        transporter.transform.parent = wingsPoint.transform;
        createdDrawObj.GetComponent<MeshRenderer>().enabled = true;
        isDrawComeFromOutside = true;

        pointCount = 0;
        //Destroy 2d render object on panel
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


