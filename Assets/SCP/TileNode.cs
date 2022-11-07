using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileNode 
{
    public bool walkable;
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;
    public TileNode parent;

    public int gCost;
    public int hCost;
    
    public TileNode(bool _walkable , Vector3 _worldpos,int _gridx,int _gridy )
    {
        walkable = _walkable;
        worldPosition = _worldpos;
        gridX = _gridx;
        gridY = _gridy;
    }
    
    public int fCost { get { return gCost + hCost; } }
}
