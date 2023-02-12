using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class Node
{
    //Used for Pathfinding
    private Vector3 position;
    private Vector2Int gridIndex;
    private bool walkable;
    internal int hCost {get; private set;} = 0; // distance to the end node
    internal int gCost { get; private set; } = 0; // distance from starting node
    //fCost is the gCost + the hCost
    internal int fCost {   
        get { return gCost + hCost; }
        private set {}
    }
    internal Node pathParent { get; private set; } = null;
    internal Node(Vector3 position, Vector2Int gridIndex, bool walkable)
    {
        this.position = position;
        this.gridIndex = gridIndex;
        this.walkable = walkable;
    }

    internal void ResetNode()
    {
        hCost = 0;
        gCost = 0;
        pathParent = null;
    }

    internal Vector3 GetPosition()
    {
        return position;
    }
    internal Vector2Int GetGridIndex()
    {
        return gridIndex;
    }

    internal bool GetWalkable()
    {
        return walkable;
    }

    internal void SetHCost(int hCost)
    {
        this.hCost = hCost;
    }
    internal void SetGCost(int gCost)
    {
        this.gCost = gCost;
    }

    internal void SetPathParent(Node parent)
    {
        pathParent = parent;
    }

}
