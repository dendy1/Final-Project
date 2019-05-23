using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro.EditorUtilities;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private Grid _grid;

    private void Awake()
    {
        _grid = GetComponent<Grid>();
    }

    public void StartFindPath(Vector3 startPosition, Vector3 targetPosition)
    {
        StartCoroutine(FindPath(startPosition, targetPosition));
    }

    private IEnumerator FindPath(Vector3 startPosition, Vector3 targetPosition)
    {
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccessful = false;
        
        Node startNode = _grid.GetNodeFromWorldPosition(startPosition);
        Node targetNode = _grid.GetNodeFromWorldPosition(targetPosition);
        startNode.Parent = startNode;

        if (startNode.Walkable && targetNode.Walkable)
        {
            BinaryHeap<Node> openHeap = new BinaryHeap<Node>(_grid.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();
            openHeap.Insert(startNode);
        
            while (openHeap.Count > 0)
            {
                Node currentNode = openHeap.Extract();
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    pathSuccessful = true;
                    break;
                }

                foreach (var neighbour in _grid.GetNeighbours(currentNode))
                {
                    if (!neighbour.Walkable || closedSet.Contains(neighbour))
                        continue;

                    int newMovementCostToNeighbour = currentNode.GCost + currentNode.GetDistance(neighbour) + neighbour.MovementPenalty;
                
                    if (newMovementCostToNeighbour < neighbour.GCost || !openHeap.Contains(neighbour))
                    {
                        neighbour.GCost = newMovementCostToNeighbour;
                        neighbour.HCost = neighbour.GetDistance(targetNode);
                        neighbour.Parent = currentNode;
                    
                        if (!openHeap.Contains(neighbour))
                            openHeap.Insert(neighbour);
                        else
                            openHeap.UpdateItem(neighbour);
                    }
                }
            }
        }

        yield return null;

        if (pathSuccessful)
        {
            waypoints = RetracePath(startNode, targetNode);
        }
        
        PathRequestManager.Instance.OnFinishedProcessingPath(waypoints, pathSuccessful);
    }

    private Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }

        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        
        return waypoints;
    }

    private Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 lastDirection = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 newDirection = new Vector2(path[i - 1].GridPositionX - path[i].GridPositionX, path[i - 1].GridPositionY - path[i].GridPositionY);

            if (newDirection != lastDirection)
            {
                waypoints.Add(path[i].WorldPosition);
            }

            lastDirection = newDirection;
        }

        return waypoints.ToArray();
    }
}
