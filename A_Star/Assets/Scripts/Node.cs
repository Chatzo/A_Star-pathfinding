using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class Node
{
    // GameObject/Tile that this node is connected to in Unity
    internal Tile tile { get; private set; }

    //Used for Pathfinding
    private Vector3 position;
    private int hValue = 0;
    private int gValue = 0;
    internal int fValue { 
        get { return gValue + hValue; }
        private set {}
    }
    internal Node pathParent = null;
    internal Node(Vector3 position)
    {
        this.position = position;
    }
    internal Node(Tile tile)
    {
        this.tile = tile;
        this.position = tile.transform.position;
    }
    private void ResetNodePathValues()
    {
        hValue = 0;
        gValue = 0;
        pathParent = null;
    }

}
