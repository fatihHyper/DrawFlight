using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class GridManager : Singleton<GridManager>
{
    [HideInInspector]
    public GridTileEvent OnGridTileCreated = new GridTileEvent();


    public GridData GridData;

    public GridTile[,] Grid;

    private int width { get { return GridData.Width; } }
    private int height { get { return GridData.Height; } }

    private float GridOffset { get { return GridData.GridOffset; } }
    private Vector3 StartPos { get { return GridData.StartPos; } }

    private GameObject gridPrefab { get { return GridData.GridPrefab; } }
    private Vector3 gridPrefabSize;
    private Vector3 prefabOffset;

    

    private void OnEnable()
    {
        if (Managers.Instance == null)
            return;

        SceneController.Instance.OnSceneLoaded.AddListener(SetGridValues); 
        OnGridTileCreated.AddListener(AddGridTile);                        // for grids created in runtime
    }

    private void OnDisable()
    {
        if (Managers.Instance == null)
            return;

        SceneController.Instance.OnSceneLoaded.RemoveListener(SetGridValues);
        OnGridTileCreated.RemoveListener(AddGridTile);
    }


    public void SetGridValues()
    {
        MeshRenderer GridPrefabRenderer = gridPrefab.GetComponent<MeshRenderer>();
        gridPrefabSize = GridPrefabRenderer.bounds.size;
        Grid = new GridTile[width, height];

        prefabOffset = new Vector3(gridPrefabSize.x / 2, -gridPrefabSize.y / 2, -gridPrefabSize.z / 2);

        if (GridData.GridPivotPoint == GridPivotPoint.Center)
        {
            prefabOffset.x += -(width * gridPrefabSize.x) / 2;
            if (GridData.GridType == GridType.VerticalGrid)
                prefabOffset.y += (width * gridPrefabSize.y) / 2;
            if(GridData.GridType == GridType.HorizantalGrid)
                prefabOffset.z += (width * gridPrefabSize.z) / 2;
        }
             
    }

    public void GenerateGrid(GameObject GridParent)
    {

        SetGridValues();


        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {

#if UNITY_EDITOR
                GameObject gridObj = UnityEditor.PrefabUtility.InstantiatePrefab(gridPrefab) as GameObject;
                gridObj.transform.position = GetWorldPosition(x, y);
                gridObj.transform.rotation = Quaternion.identity;

#else
                GameObject gridObj = Instantiate(gridPrefab,  GetWorldPosition(x,y) , Quaternion.identity);

#endif
                gridObj.name = "Grid_" + x + "_" + y;
                gridObj.transform.SetParent(GridParent.transform);

                if (gridObj.GetComponent<GridTile>() == null)
                {
                    GridTile gridTile = gridObj.AddComponent<GridTile>();
                    gridTile.SetTile(x, y, GridData);
                }
            }

        }
    }


    public void GenerateGrid(GridData gridData , GameObject GridParent)
    {
        GridData = gridData;
        GenerateGrid(GridParent);
    }

    public void AddGridTile( GridTile gridTile)
    {
        int x = gridTile.X;
        int y = gridTile.Y;

        Grid[x, y] = gridTile;
    }

    public void RemoveGridTile(GridTile gridTile)
    {
        int x = gridTile.X;
        int y = gridTile.Y;

        Grid[x, y] = null;
        Destroy(Grid[x, y].gameObject);

    }

    public void DestroyGridTile(GridTile gridTile)
    {
        int x, y;
        GetXY(gridTile, out x, out y);

        Destroy(Grid[x, y].gameObject);
        Grid[x, y] = null;
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        //SetGridValues();
        Vector3 worldPos = Vector3.zero;
        worldPos.x = x * (gridPrefabSize.x + GridOffset);
        if (GridData.GridType == GridType.VerticalGrid)
            worldPos.y = -y * (gridPrefabSize.y + GridOffset);
        if (GridData.GridType == GridType.HorizantalGrid)
            worldPos.z = -y * (gridPrefabSize.z + GridOffset);

        return worldPos + StartPos + prefabOffset;
    }

    public void GetXY(GridTile gridTile, out int x, out int y)
    {
        x = gridTile.X;
        y = gridTile.Y;

    }
    public bool IsXYValid(int x, int y)
    {
        return (x >= 0 && y >= 0) && (x < width && y < height);
    }


}

public class GridTileEvent : UnityEvent<GridTile> { }