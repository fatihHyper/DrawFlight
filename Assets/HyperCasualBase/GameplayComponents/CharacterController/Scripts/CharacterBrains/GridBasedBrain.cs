using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBasedBrain : CharacterBrainBase
{
    private GridTile targetTile;
    int nextTileIndex = 1;
    private bool PathFound;
    private bool PathReached;
    private bool newTarget;
    private List<Vector3> pathPositions;
    private List<GridTile> path;

    private void OnEnable()
    {
        if (Managers.Instance == null)
            return;

        PathFindingManager.Instance.NewTargetTile.AddListener(SetTarget);
    }

    private void OnDisable()
    {
        if (Managers.Instance == null)
            return;

        PathFindingManager.Instance.NewTargetTile.RemoveListener(SetTarget);
    }


    private void SetTarget(GridTile _targetTile)
    {
        targetTile = _targetTile;
        newTarget = true;
    }


    public override void Logic()
    {
        if (newTarget && FindTileStandingOn() != null)
        {
            if (!PathFound)
                FindPath();

            else if (!PathReached)
                MoveOnPath();
        }
    }

    private void FindPath()
    {
        path = PathFindingManager.Instance.FindPath(GridManager.Instance.Grid, FindTileStandingOn(), targetTile);

        if (path == null || path.Count == 1)
            return;


        PathFindingManager.Instance.HighlightPath(path);
        pathPositions = PathFindingManager.Instance.GetPathPositions(path);



        PathFound = true;
        PathReached = false;
    }

    private GridTile FindTileStandingOn()
    {
        RaycastHit hit;
        //Debug.DrawRay(transform.position + new Vector3(0, 0.1f, 0.2f), -transform.up * 10f, Color.yellow);
        if (Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0.2f), -transform.up, out hit, 10f, GridManager.Instance.GridData.GridLayer))
        {
            return hit.collider.gameObject.GetComponent<GridTile>();
        }
        return null;
    }



    private void MoveOnPath()
    {

        if (Vector3.Distance(transform.position, pathPositions[nextTileIndex]) >= 0.6f)
        {
            CharacterController.Move(pathPositions[nextTileIndex] /*- transform.position*/);
            //transform.position = Vector3.MoveTowards(transform.position, pathPositions[nextTileIndex], 10f * Time.deltaTime);
        }
        else
        {
            CharacterController.Move(transform.position);
            nextTileIndex++;

            if (nextTileIndex == path.Count)
            {
                //end tile reached
                PathReached = true;
                PathFound = false;
                newTarget = false;
                nextTileIndex = 1;
            }
        }


    }
}
