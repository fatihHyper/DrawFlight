using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PathFindingInputControls : MonoBehaviour
{
    [InfoBox("Add PathFindingManager to Managers")]
    public bool ManuallySetStartTile;

    [ShowIfGroup("ManuallySetStartTile")]
    [BoxGroup("ManuallySetStartTile/Set StartTile Manually")]
    public GridTile StartTile;

    private GridTile startTile;
    private GridTile endTile;


    private void Update()
    {
        SetTarget();
        SetTileColorsToDefault();
    }

    public void SetTarget()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                endTile = hit.collider.gameObject.GetComponent<GridTile>();
                PathFindingManager.Instance.NewTargetTile.Invoke(endTile);
            }
        }
    }

    public void SetTileColorsToDefault()
    {
        if (Input.GetMouseButtonUp(0))
        {
            GridTile[,] gridTiles = GridManager.Instance.Grid;

            for (int x = 0; x < gridTiles.GetLength(0); x++)
            {
                for (int y = 0; y < gridTiles.GetLength(1); y++)
                {
                    if (gridTiles[x, y].IsWalkable)
                        gridTiles[x, y].gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
                }
            }
        }
    }

    public void FindPathFromZeroToClickPoint()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                startTile = GridManager.Instance.Grid[0, 0];
                endTile = hit.collider.gameObject.GetComponent<GridTile>();
                PathFindingManager.Instance.NewTargetTile.Invoke(endTile);

                FindPath(startTile, endTile);
               
            }
        }
    }

    public void SetStartPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            startTile = hit.collider.gameObject.GetComponent<GridTile>();
        }
    }


    [ShowIfGroup("ManuallySetStartTile")]
    [BoxGroup("ManuallySetStartTile/Set StartTile Manually")]
    [Button("Set StartTile")]
    public void SetStartTileManually()
    {
        startTile = StartTile;
    }

    public void SetEndPoint()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                endTile = hit.collider.gameObject.GetComponent<GridTile>();
                FindPath(startTile, endTile);
            }

        }
    }

    public void FindPath(GridTile startTile , GridTile endTile)
    {
        List<GridTile> path = PathFindingManager.Instance.FindPath(GridManager.Instance.Grid, startTile, endTile);

        if (path != null)
        {
            for (int i = 0; i < path.Count; i++)
            {
                path[i].gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
            }
        }
    }

}
