using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    //public Transform player;
    public LayerMask unwalkbleMask; //벽 유무 판별
    public Vector2 gridWorldSize; //전체 맵 크기
    public float nodeRadius; //하나의 노드 반지름
    TileNode[,] grid; //그려줄 그리드

    float nodeDiameter; //하나의 노드 지름
    int gridSizeX, gridSizeY; // 그려줄 노드의 개수

    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt( gridWorldSize.x /nodeDiameter); //반올림하여 float 값을 int에 저장 
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new TileNode[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2; // 맵의 정중앙부터 왼쪽 -> 아래 즉 왼쪽 아래 구석으로 위치시킴

        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (i * nodeDiameter + nodeRadius) + Vector3.forward * (j * nodeDiameter + nodeRadius); //정해준 지름대로 기즈모 위치를 지정
                
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius,unwalkbleMask));
                //콜라이더 없이 충돌체크를 약식으로 하기위한 CheckSphere  
                //지정한 레이어 unwalkbleMask에 해당하는 오브젝트와 worldPoint의 위치에서 nodeRadius만큼 충돌체크
                grid[i, j] = new TileNode(walkable, worldPoint,i,j);
            }
        }
    }

    public List<TileNode> GetNeighbours(TileNode node)
    {
        List<TileNode> neighbours = new List<TileNode>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }


    

    public TileNode NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        
        int x = Mathf.RoundToInt( (gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    public List<TileNode> path;
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        if (grid != null)
        {
            //TileNode playerNode = NodeFromWorldPoint(player.position);
            foreach (TileNode n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                if (path != null)
                    if(path.Contains(n))
                    Gizmos.color = Color.black;
                //if (playerNode == n)
                //   Gizmos.color = Color.cyan;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f)); // 0.1를 지름에서 빼서 너무 꽉차보이지 않고 약간 띄엄띄엄 기즈모를 그려줌
            }
            
            
        }
    }
}
