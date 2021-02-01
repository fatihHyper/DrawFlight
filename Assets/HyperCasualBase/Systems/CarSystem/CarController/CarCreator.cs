using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


[SerializeField]
public struct SpawnInfo
{
    public Vector3 position;
    public Quaternion rotation;

    public SpawnInfo(Vector3 position, Quaternion rotation)
    {
        this.position = position;
        this.rotation = rotation;
    }
}
public class CarCreator : MonoBehaviour
{
    [SerializeField]
    [ValueDropdown("CarDatasDropdown")]
    private CarData carData;

    private SpawnInfo spawnInfo;


    private void OnEnable()
    {
        if (Managers.Instance == null)
            return;

        SceneController.Instance.OnSceneLoaded.AddListener(CreateCar);
    }

    private void OnDisable()
    {
        if (Managers.Instance == null)
            return;

        SceneController.Instance.OnSceneLoaded.RemoveListener(CreateCar);
    }

    private void OnDrawGizmos()
    {
        if (carData == null || carData.CarGraphicData.BodyMesh == null)
            return;
        Gizmos.color = Color.red;
        Vector3 size = carData.CarGraphicData.BodyMesh.bounds.size;

        var rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.matrix = rotationMatrix;


        spawnInfo = new SpawnInfo(transform.position, transform.rotation);
        Gizmos.DrawWireCube(Vector3.zero, size);
    }

#if UNITY_EDITOR
    private static IEnumerable CarDatasDropdown()
    {
        return UnityEditor.AssetDatabase.FindAssets("t:CarData")
            .Select(x => UnityEditor.AssetDatabase.GUIDToAssetPath(x))
            .Select(x => new ValueDropdownItem(x, UnityEditor.AssetDatabase.LoadAssetAtPath<CarData>(x)));
    }
#endif


    public void CreateCar()
    {
        CarManager.Instance.CreateCar(carData,spawnInfo.position, spawnInfo.rotation);
    }

}
