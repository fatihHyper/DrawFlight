using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour
{
    public int X, Y;

    public int GCost;
    public int HCost;
    public int FCost;

    public GridData GridData;
    public bool IsWalkable; /*{ get { return GridData.WalkableTable[X,Y]; } }*/
    public GridTile CameFromTile;


    private void OnEnable()
    {
        if (Managers.Instance == null)
            return;

        SceneController.Instance.OnSceneLoaded.AddListener(() => GridManager.Instance.AddGridTile(this)); // for grids created in editor
        LevelManager.Instance.OnLevelFinish.AddListener(() => GridManager.Instance.RemoveGridTile(this));
    }

    private void OnDisable()
    {
        if (Managers.Instance == null)
            return;

        SceneController.Instance.OnSceneLoaded.RemoveListener(() => GridManager.Instance.AddGridTile(this));
        LevelManager.Instance.OnLevelFinish.RemoveListener(() => GridManager.Instance.RemoveGridTile(this));
    }

    private void Start()
    {
        if (!IsWalkable)
            gameObject.GetComponent<MeshRenderer>().material.color = Color.black;
    }
    public void SetTile(int _x, int _y, GridData gridData)
    {
        X = _x;
        Y = _y;
        GridData = gridData;
        IsWalkable = gridData.WalkableTable[X, Y];
        gameObject.layer = LayerMask.NameToLayer("Ground");
        GridManager.Instance.OnGridTileCreated.Invoke(this);
    }


    public void CalculateFCost()
    {
        FCost = GCost + HCost;
    }

}
