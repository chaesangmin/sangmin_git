using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Transform seeker, target;
    Grid grid;
    private void Awake()
    {
        grid = GetComponent<Grid>();
    }
    private void Update()
    {
        FindPath(seeker.position, target.position);
    }
    void FindPath(Vector3 startPos,Vector3 targetPos)
    {
        TileNode startNode = grid.NodeFromWorldPoint(startPos);
        TileNode targetNode = grid.NodeFromWorldPoint(targetPos);

        List<TileNode> openSet = new List<TileNode>();
        HashSet<TileNode> closedSet = new HashSet<TileNode>();
        openSet.Add(startNode);

        while(openSet.Count > 0)
        {
            TileNode currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++) 
            {
                if(openSet[i].fCost < currentNode.fCost
                    || openSet[i].fCost == currentNode.fCost
                    && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if(currentNode == targetNode)
            {
                RetracePath(startNode,targetNode);
                return;
            }

            foreach (TileNode neighbour in grid.GetNeighbours(currentNode))
            {
                if(!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if(newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
    }
    void RetracePath(TileNode startNode, TileNode endNode)
    {
        List<TileNode> path = new List<TileNode>();
        TileNode currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        grid.path = path;

    }

    int GetDistance(TileNode nodeA,TileNode nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
