using UnityEngine;
using UnityEngine.AI;
using Dreamteck.Splines;

public class SwipeControl : MonoBehaviour
{

    private static NavMeshSurface navMeshSurface;
    private static NavMeshSurface NavMeshSurface { get { return (navMeshSurface == null) ? navMeshSurface = FindObjectOfType<NavMeshSurface>() : navMeshSurface; } set { navMeshSurface = value; } }


    private GameObject m_rendererPrefab;
    private GameObject obstacleInRender;
    private GameObject m_currentRenderer;

    public GameObject drawPanel;
    public GameObject wingsPoint;
    private GameObject wingsParent;

    private int layer_mask;
    private bool isDrawComeFromOutside;
    public GameObject testObject;
    private GameObject test1;
    public Material material;
    public Mesh Wallmesh;

    private Vector3 startPos;
    private Vector3 lastPos;
    private Plane m_cast;
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
                creatSplineObject(hit);
                startDrawing(hit);
                //PoolingSystem.Instance.InstantiateAPS("StarExplosion", hit.point);

            }
        }
        else if (IsInput(TouchPhase.Moved) && LevelManager.Instance.IsLevelStarted)
        {

            Ray _ray = m_camera.ScreenPointToRay(Input.touchCount == 1 ? (Vector3)Input.mousePosition : Input.mousePosition);

            
            if (Physics.Raycast(_ray, out hit, layer_mask)) 
            {
               
                    if (Vector3.Distance(lastPos, hit.point) >= 1 && pointCount < points.Length)
                    {
                        drawWithMove(hit);
                        //PoolingSystem.Instance.InstantiateAPS("StarExplosion", hit.point);
                    }
                
            }
        }
        else if (IsInput(TouchPhase.Ended))
        {

            test1.transform.position = wingsParent.transform.position;
             spline.Break(pointCount-1);
        }
    }

  
    void creatSplineObject(RaycastHit hit)
    {
        startPos = hit.point;
        lastPos = startPos;

        wingsParent = (GameObject)Instantiate(testObject,wingsPoint.transform.position,Quaternion.identity,drawPanel.transform);

        test1 = (GameObject)Instantiate(testObject, startPos, Quaternion.identity,wingsParent.transform);
        test1.layer = LayerMask.NameToLayer("Ignore Raycast");
        test1.GetComponent<MeshRenderer>().material = material;

        spline = test1.AddComponent<SplineComputer>();
        //spline.uniformScale = false;
        Dreamteck.Splines.SplineMesh splineMesh = test1.AddComponent<Dreamteck.Splines.SplineMesh>();
        splineMesh.updateMethod = SplineUser.UpdateMethod.FixedUpdate;
        spline.updateMode = SplineComputer.UpdateMode.FixedUpdate;


        splineMesh.AddChannel(Wallmesh, "Wall");
        splineMesh.GetChannel(0).type = Dreamteck.Splines.SplineMesh.Channel.Type.Place;
        splineMesh.GetChannel(0).count = 80;
        splineMesh.GetChannel(0).minScale = new Vector3(1f, 0.1f, 0.3f);
        splineMesh.GetChannel(0).maxScale = new Vector3(1f, 0.1f, 0.3f);
        points = new SplinePoint[50];

        spline.type = Spline.Type.BSpline;
        spline.sampleMode = SplineComputer.SampleMode.Uniform;
        spline.customNormalInterpolation = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        spline.customValueInterpolation = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
    }
    void startDrawing(RaycastHit hit)
    {
        if ( hit.collider == null)
        {
            isDrawComeFromOutside = true;
            Debug.Log("Its outside");
        }
        else
        {
            isDrawComeFromOutside = false;
            points[pointCount] = new SplinePoint();
            points[pointCount].position = hit.point;
            points[pointCount].normal = Vector3.up;
            points[pointCount].size = 0.5f;
            points[pointCount].color = Color.white;

            spline.SetPoint(pointCount, points[pointCount], SplineComputer.Space.World);
            NavMeshSurface.UpdateNavMesh(NavMeshSurface.navMeshData);
            lastPos = hit.point;
            pointCount++;

        }
    }
    void drawWithMove(RaycastHit hit)
    {
        if (!isDrawComeFromOutside)
        {
           // Debug.Log(pointCount);
            points[pointCount] = new SplinePoint();
            points[pointCount].position = hit.point;
            points[pointCount].normal = Vector3.up;
            points[pointCount].size = 0.5f;
            points[pointCount].color = Color.white;

            spline.SetPoint(pointCount, points[pointCount], SplineComputer.Space.World);
            //NavMeshSurface.UpdateNavMesh(NavMeshSurface.navMeshData);
            lastPos = hit.point;
            pointCount++;
        }
       

       
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


