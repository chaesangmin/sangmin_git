using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    //public Transform player;
    public LayerMask unwalkbleMask; //�� ���� �Ǻ�
    public Vector2 gridWorldSize; //��ü �� ũ��
    public float nodeRadius; //�ϳ��� ��� ������
    TileNode[,] grid; //�׷��� �׸���

    float nodeDiameter; //�ϳ��� ��� ����
    int gridSizeX, gridSizeY; // �׷��� ����� ����

    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt( gridWorldSize.x /nodeDiameter); //�ݿø��Ͽ� float ���� int�� ���� 
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new TileNode[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2; // ���� ���߾Ӻ��� ���� -> �Ʒ� �� ���� �Ʒ� �������� ��ġ��Ŵ

        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (i * nodeDiameter + nodeRadius) + Vector3.forward * (j * nodeDiameter + nodeRadius); //������ ������� ����� ��ġ�� ����
                
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius,unwalkbleMask));
                //�ݶ��̴� ���� �浹üũ�� ������� �ϱ����� CheckSphere  
                //������ ���̾� unwalkbleMask�� �ش��ϴ� ������Ʈ�� worldPoint�� ��ġ���� nodeRadius��ŭ �浹üũ
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
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f)); // 0.1�� �������� ���� �ʹ� ���������� �ʰ� �ణ ������ ����� �׷���
            }
            
            
        }
    }
}
