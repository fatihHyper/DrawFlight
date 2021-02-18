using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{

    private MaterialPropertyBlock materialProperty;

    public List<Transform> conffetiTransforms = new List<Transform>();

    public Color StartColor = Color.white;
    public Color SuccessColor = Color.white;

    private MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        materialProperty = new MaterialPropertyBlock();

        materialProperty.SetColor("_Color", StartColor);
        meshRenderer.SetPropertyBlock(materialProperty);
    }


    public void PassObstacle()
    {
        
        foreach (var item in conffetiTransforms)
        {
            PoolingSystem.Instance.InstantiateAPS("Confetti", item.position, item.rotation);
        }

        materialProperty.SetColor("_Color", SuccessColor);
        meshRenderer.SetPropertyBlock(materialProperty);

    }

}
