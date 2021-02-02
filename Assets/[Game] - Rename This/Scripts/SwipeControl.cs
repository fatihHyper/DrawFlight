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
                creatSplineObject(hit);
                startDrawing(hit);
                //PoolingSystem.Instance.InstantiateAPS("StarExplosion", hit.point);
                isDrawComeFromOutside = false;

                Transform[] wingsList = wingsPoint.GetComponentsInChildren<Transform>();

                for (int i = 0; i < wingsList.Length; i++)
                {
                    wingsList[i].gameObject.SetActive(false);
                }
                wingsPoint.SetActive(true);
                m_origin = hit.point;
                m_currentRenderer = (GameObject)Instantiate(m_rendererPrefab, m_origin, Quaternion.identity,wingsPoint.transform);

            }
        }
        else if (IsInput(TouchPhase.Moved) && LevelManager.Instance.IsLevelStarted && !isDrawComeFromOutside)
        {

            Ray _ray = m_camera.ScreenPointToRay(Input.touchCount == 1 ? (Vector3)Input.mousePosition : Input.mousePosition);

            
            if (Physics.Raycast(_ray, out hit, layer_mask)) 
            {
               
                    if (Vector3.Distance(lastPos, hit.point) >= 1 && pointCount < points.Length)
                    {
                        drawWithMove(hit);
                    //PoolingSystem.Instance.InstantiateAPS("StarExplosion", hit.point);

                    m_currentRenderer.transform.position = hit.point;
                }
                
            }
        }
        else if (IsInput(TouchPhase.Ended))
        {
            //createdDrawObj.transform.localEulerAngles = Vector3.zero;
            createdDrawObj.GetComponent<MeshRenderer>().enabled = true;
            
            isDrawComeFromOutside = true;
            clone = Instantiate(transformPanel, wingsPoint.transform.position, Quaternion.Euler(Vector3.zero));
            
            clone.transform.position = wingsPoint.transform.position;
            //clone.transform.localEulerAngles = new Vector3(90, 0, 0);
            clone.transform.parent = wingsPoint.transform;

            //clone.GetComponentInChildren<SplineComputer>().gameObject.transform.localPosition = Vector3.zero;
            //clone.GetComponentInChildren<SplineComputer>().gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
            createdDrawObj.transform.localEulerAngles = new Vector3(135,0,0);
            createdDrawObj.transform.localPosition = clone.transform.position;


            pointCount = 0;
            Destroy(m_currentRenderer);
            Destroy(createdDrawObj);

        }
    }

  
    void creatSplineObject(RaycastHit hit)
    {
        startPos = hit.point;
        lastPos = startPos;


        createdDrawObj = (GameObject)Instantiate(drawObjPref, startPos, Quaternion.identity, transformPanel.transform);
        createdDrawObj.layer = LayerMask.NameToLayer("Ignore Raycast");
        createdDrawObj.GetComponent<MeshRenderer>().material = material;
        createdDrawObj.GetComponent<MeshRenderer>().enabled = false;
        spline = createdDrawObj.AddComponent<SplineComputer>();
       
        //spline.uniformScale = false;
        Dreamteck.Splines.SplineMesh splineMesh = createdDrawObj.AddComponent<Dreamteck.Splines.SplineMesh>();
        splineMesh.updateMethod = SplineUser.UpdateMethod.FixedUpdate;
        spline.updateMode = SplineComputer.UpdateMode.FixedUpdate;


        splineMesh.AddChannel(Wallmesh, "Wall");
        splineMesh.GetChannel(0).type = Dreamteck.Splines.SplineMesh.Channel.Type.Place;
        splineMesh.GetChannel(0).count = 80;
        splineMesh.GetChannel(0).minScale = new Vector3(0.1f, 1f, 0.1f);
        splineMesh.GetChannel(0).maxScale = new Vector3(0.1f, 1f, 0.1f);
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
            points[pointCount].size = 1f;
            points[pointCount].color = Color.white;

            spline.SetPoint(pointCount, points[pointCount], SplineComputer.Space.World);
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
            points[pointCount].size = 1f;
            points[pointCount].color = Color.white;
            
            spline.SetPoint(pointCount, points[pointCount], SplineComputer.Space.World);
           
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


