using UnityEngine;
using UnityEngine.AI;
using Dreamteck.Splines;

public class SwipeControl : MonoBehaviour
{

    public GameObject m_rendererPrefab;
    private GameObject m_currentRenderer;

    private GameObject clone;
    public GameObject transformPanel;
    public GameObject wingsPoint;
    private GameObject wingsParent;

    private int layer_mask;
    private bool isDrawComeFromOutside;
    public GameObject drawObjPref;
    private GameObject createdDrawObj;
    public Material material;
    public Mesh Wallmesh;

    private Vector3 startPos;
    private Vector3 lastPos;
    private Plane m_cast;
    private Vector3 m_origin;
    [SerializeField] private Camera m_camera;

    private MeshScaleModifier MeshScaleModifier = new MeshScaleModifier();
    private int pointCount;
    private int lineCount;
    private SplineComputer spline;
    private SplinePoint[] points;
    private Dreamteck.Splines.SplineMesh splineMesh;


    private void Awake()
    {
        layer_mask = LayerMask.GetMask("Ground");
        lineCount = 0;
        pointCount = 0;
        m_cast = new Plane(m_camera.transform.forward * -1, this.transform.position);
        // NavMeshSurface.UpdateNavMesh(NavMeshSurface.navMeshData);
        //obstacleInRender = (GameObject)PoolingSystem.Instance.InstantiateAPS("DrawCube");
        // m_rendererPrefab = (GameObject) PoolingSystem.Instance.InstantiateAPS("SwipeDraw");

    }



    private void FixedUpdate()
    {
        RaycastHit hit;

        if (IsInput(TouchPhase.Began) && LevelManager.Instance.IsLevelStarted)
        {
            Ray _ray = m_camera.ScreenPointToRay(Input.touchCount == 1 ? (Vector3)Input.mousePosition : Input.mousePosition);

            if (Physics.Raycast(_ray, out hit, layer_mask))
            {
                CreatSplineObject(hit);
                StartDrawing(hit);
                //PoolingSystem.Instance.InstantiateAPS("StarExplosion", hit.point);
                
            }

            m_currentRenderer = (GameObject)Instantiate(m_rendererPrefab, hit.point, Quaternion.identity, wingsPoint.transform);
        }
        else if (IsInput(TouchPhase.Moved) && LevelManager.Instance.IsLevelStarted && !isDrawComeFromOutside)
        {

            Ray _ray = m_camera.ScreenPointToRay(Input.touchCount == 1 ? (Vector3)Input.mousePosition : Input.mousePosition);


            if (Physics.Raycast(_ray, out hit, layer_mask))
            {
                
                if (Vector3.Distance(lastPos, hit.point) >= 0.5f && pointCount < points.Length)
                {
                    DrawWithMove(hit);
                    //PoolingSystem.Instance.InstantiateAPS("StarExplosion", hit.point);

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
        clone = null;
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
        splineMesh.GetChannel(0).minScale = new Vector3(0.1f, 1f, 0.1f);
        splineMesh.GetChannel(0).maxScale = new Vector3(0.1f, 1f, 0.1f);
        points = new SplinePoint[50];
        spline.type = Spline.Type.BSpline;
        spline.sampleMode = SplineComputer.SampleMode.Uniform;
        spline.customNormalInterpolation = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        spline.customValueInterpolation = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

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

        //Make disable all children of wingsPoint
        Transform[] wingsList = wingsPoint.GetComponentsInChildren<Transform>();

        for (int i = 0; i < wingsList.Length; i++)
        {
            wingsList[i].gameObject.SetActive(false);
            //Or Destroy
        }
        //Control for wingsPoint is enable
        wingsPoint.SetActive(true);


        
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

            //2D draw with MouseMove
            m_currentRenderer.transform.position = hit.point;
            m_currentRenderer.layer = 12;
        }
    }

    void EndOfDraw()
    {
        createdDrawObj.GetComponent<MeshRenderer>().enabled = true;
        isDrawComeFromOutside = true;

        //create copy of transformPanel and change position to on Fligt Wings
        clone = Instantiate(transformPanel, wingsPoint.transform.position, Quaternion.Euler(Vector3.zero));
        clone.transform.position = wingsPoint.transform.position;
        clone.transform.parent = wingsPoint.transform;

        clone.transform.GetChild(0).transform.localPosition = new Vector3(createdDrawObj.transform.position.x, 0, 0);
        

        
        pointCount = 0;

        //Destroy original draw obj. and 2d render object
        Destroy(m_currentRenderer);
        Destroy(createdDrawObj);
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


