using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PathFindingManager : Singleton<PathFindingManager>
{
    [HideInInspector]
    public GridTileEvent NewTargetTile = new GridTileEvent();


    private GridTile[,] grid;

    private List<GridTile> openList;
    private List<GridTile> closedList;

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;


    public List<GridTile> FindPath(GridTile[,] _grid, GridTile startTile, GridTile endTile)
    {
        grid = _grid;

        if (grid ==null || startTile == null || endTile == null)
            return null;// Invalid Path

        openList = new List<GridTile> { startTile };
        closedList = new List<GridTile>();


        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                GridTile gridTile = grid[x, y];
                gridTile.GCost = int.MaxValue;
                gridTile.CalculateFCost();
                gridTile.CameFromTile = null;
            }
        }

        startTile.GCost = 0;
        startTile.HCost = CalculateDistanceCost(startTile, endTile);
        startTile.CalculateFCost();

        while (openList.Count > 0)
        {
            GridTile currentTile = GetLowestFCostTile(openList);
            if (currentTile == endTile)
            {
                // Reached final node
                //visual
                return CalculatePath(endTile);
               
            }

            openList.Remove(currentTile);
            closedList.Add(currentTile);

            foreach (GridTile neighbourTile in GetNeighbourList(currentTile))
            {
                if (closedList.Contains(neighbourTile)) continue;
                if (!neighbourTile.IsWalkable)
                {
                    closedList.Add(neighbourTile);
                    continue;
                }

                int tentativeGCost = currentTile.GCost + CalculateDistanceCost(currentTile, neighbourTile);
                if (tentativeGCost < neighbourTile.GCost)
                {
                    neighbourTile.CameFromTile = currentTile;
                    neighbourTile.GCost = tentativeGCost;
                    neighbourTile.HCost = CalculateDistanceCost(neighbourTile, endTile);
                    neighbourTile.CalculateFCost();

                    if (!openList.Contains(neighbourTile))
                    {
                        openList.Add(neighbourTile);
                    }
                }
                //visual
                //PathfindingDebugStepVisual.Instance.TakeSnapshot(grid, currentTile, openList, closedList);
            }
        }

        // Out of nodes on the openList
        return null;

    }


    private int CalculateDistanceCost(GridTile a, GridTile b)
    {
        int xDistance = Mathf.Abs(a.X - b.X);
        int yDistance = Mathf.Abs(a.Y - b.Y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private GridTile GetLowestFCostTile(List<GridTile> pathList)
    {
        GridTile lowestFCostTile = pathList[0];
        for (int i = 1; i < pathList.Count; i++)
        {
            if (pathList[i].FCost < lowestFCostTile.FCost)
            {
                lowestFCostTile = pathList[i];
            }
        }
        return lowestFCostTile;
    }

    private List<GridTile> CalculatePath(GridTile endTile)
    {
        List<GridTile> path = new List<GridTile>();
        path.Add(endTile);
        GridTile currentTile = endTile;
        while (currentTile.CameFromTile != null)
        {
            path.Add(currentTile.CameFromTile);
            currentTile = currentTile.CameFromTile;
        }
        path.Reverse();
        GetPathPositions(path);
        return path;
    }

    private List<GridTile> GetNeighbourList(GridTile currentTile)
    {
        List<GridTile> neighbourList = new List<GridTile>();

        if (currentTile.X - 1 >= 0)
        {
            // Left
            neighbourList.Add(GetGridTile(currentTile.X - 1, currentTile.Y));

            if (currentTile.GridData.IsDiagonalPathEnabled)
            {
                // Left Down
                if (currentTile.Y - 1 >= 0) neighbourList.Add(GetGridTile(currentTile.X - 1, currentTile.Y - 1));
                // Left Up
                if (currentTile.Y + 1 < grid.GetLength(1)) neighbourList.Add(GetGridTile(currentTile.X - 1, currentTile.Y + 1));
            }
           
        }
        if (currentTile.X + 1 < grid.GetLength(0))
        {
            // Right
            neighbourList.Add(GetGridTile(currentTile.X + 1, currentTile.Y));

            if (currentTile.GridData.IsDiagonalPathEnabled)
            {
                // Right Down
                if (currentTile.Y - 1 >= 0) neighbourList.Add(GetGridTile(currentTile.X + 1, currentTile.Y - 1));
                // Right Up
                if (currentTile.Y + 1 < grid.GetLength(1)) neighbourList.Add(GetGridTile(currentTile.X + 1, currentTile.Y + 1));
            }
                
        }
        // Down
        if (currentTile.Y - 1 >= 0) neighbourList.Add(GetGridTile(currentTile.X, currentTile.Y - 1));
        // Up
        if (currentTile.Y + 1 < grid.GetLength(1)) neighbourList.Add(GetGridTile(currentTile.X, currentTile.Y + 1));

        return neighbourList;
    }

    public GridTile GetGridTile(int x, int y)
    {
        return grid[x, y];
    }

    public List<Vector3> GetPathPositions(List<GridTile> path)
    {
        if (path == null)
            return null;

        List<Vector3> PathPositions = new List<Vector3>();

        for (int i = 0; i < path.Count; i++)
        {
            PathPositions.Add(path[i].gameObject.transform.position);
        }        
        return PathPositions;
    }

    public void HighlightPath(List<GridTile> path)
    {
        if (path != null)
        {
            for (int i = 0; i < path.Count; i++)
            {
                path[i].gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
            }
        }
    }
}

public class PathFindingEvent : UnityEvent<List<Vector3>> { }