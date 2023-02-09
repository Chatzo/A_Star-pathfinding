using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class Node
{
    //Used for Pathfinding
    private Vector3 position;
    private bool walkable;
    private int hValue = 0; // distance to the end node
    private int gValue = 0; // distance from starting node
    //fValue is the gValue + the hValue
    internal int fValue {   
        get { return gValue + hValue; }
        private set {}
    }
    internal Node pathParent = null;
    internal Node(Vector3 position, bool walkable)
    {
        this.position = position;
        this.walkable = walkable;
    }

    private void ResetNodePathValues()
    {
        hValue = 0;
        gValue = 0;
        pathParent = null;
    }

    internal Vector3 GetPosition()
    {
        return position;
    }

    internal bool GetWalkable()
    {
        return walkable;
    }

}
