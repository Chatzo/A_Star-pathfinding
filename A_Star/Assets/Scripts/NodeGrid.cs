using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class NodeGrid : MonoBehaviour
{
    public Transform player;

    private Node[,] nodeGrid;
    private int unwalkableLayer;
    public Vector2 gridSize;
    public float nodeDistance;
    private int gridWidth;
    private int gridLength;
    private void Start()
    {
        unwalkableLayer = LayerMask.GetMask("Obstacle");
        gridWidth = Mathf.RoundToInt(gridSize.x / nodeDistance);
        gridLength = Mathf.RoundToInt(gridSize.y / nodeDistance);
        CreateGrid(gridWidth, gridLength);
    }
    private void CreateGrid(int gridWidth, int gridLength)
    {
        nodeGrid = new Node[gridWidth, gridLength];
        float nodeRadius = nodeDistance / 2f;
        Vector3 xDirection = new Vector3(gridSize.x / 2, 0f, 0f);
        Vector3 yDirection = new Vector3(0f, 0f, gridSize.y / 2);
        Vector3 bottomLeft = transform.position - xDirection - yDirection;
        Vector3 worldPosition;
        Vector3 currentX;
        Vector3 currentY;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridLength; y++)
            {
                currentX = Vector3.right * (x * nodeDistance + nodeRadius);
                currentY = Vector3.forward * (y * nodeDistance + nodeRadius);
                worldPosition = bottomLeft + currentX + currentY;
                bool walkable = true;
                if (Physics.CheckSphere(worldPosition, nodeRadius, unwalkableLayer))
                    walkable = false;
                nodeGrid[x, y] = new Node(worldPosition, new Vector2Int(x, y), walkable);
            }
        }
    }
    internal Node GetNodeFromWorldPosition(Vector3 worldPosition)
    {
        //get a position of the grid in percent of the grid.
        //for x, 0 is to the far left, 0.5 in the middle and 1 to the far right 
        //for y, 0 is the bottom, 0
        float XpositionPercent = (worldPosition.x + gridSize.x / 2) / gridSize.x;
        float YpositionPercent = (worldPosition.z + gridSize.y / 2) / gridSize.y;
        //clamp between 0 and 1 to keep it within bounds of the grid
        XpositionPercent = Mathf.Clamp01(XpositionPercent);
        YpositionPercent = Mathf.Clamp01(YpositionPercent);

        int x = Mathf.RoundToInt((gridWidth - 1) * XpositionPercent);
        int y = Mathf.RoundToInt((gridLength - 1) * YpositionPercent);
        return nodeGrid[x, y];
    }
    internal Node[] GetNeighbours(Node node)
    {
        //0,1,2
        //3,X,4
        //5,6,7
        Node[] neighbours = new Node[8];
        //UpLeft
        neighbours[0] = GetNodeAtIndex(node.GetGridIndex().x - 1, node.GetGridIndex().y + 1);
        //Up
        neighbours[1] = GetNodeAtIndex(node.GetGridIndex().x, node.GetGridIndex().y + 1);
        //UpRight
        neighbours[2] = GetNodeAtIndex(node.GetGridIndex().x + 1, node.GetGridIndex().y + 1);
        //Left
        neighbours[3] = GetNodeAtIndex(node.GetGridIndex().x - 1, node.GetGridIndex().y);
        //Right
        neighbours[4] = GetNodeAtIndex(node.GetGridIndex().x + 1, node.GetGridIndex().y);
        //DownLeft
        neighbours[5] = GetNodeAtIndex(node.GetGridIndex().x - 1, node.GetGridIndex().y - 1);
        //Down
        neighbours[6] = GetNodeAtIndex(node.GetGridIndex().x, node.GetGridIndex().y - 1);
        //DownRight
        neighbours[7] = GetNodeAtIndex(node.GetGridIndex().x + 1, node.GetGridIndex().y - 1);

        return neighbours;
    }
    private Node GetNodeAtIndex(int positionX, int positionY)
    {
        if (positionX < 0 || positionX >= gridWidth || positionY < 0 || positionY >= gridLength)
            return null;
        return nodeGrid[positionX, positionY];
    }
    //private void OnDrawGizmos()
    //{
    //    Vector3 nodeVector = new Vector3(gridSize.x, 1, gridSize.y);
    //    Gizmos.DrawWireCube(transform.position, nodeVector);

    //    if (nodeGrid != null)
    //    {
    //        Node playerNode = GetNodeFromWorldPosition(player.position);
    //        foreach (Node node in nodeGrid)
    //        {
    //            if (node.GetWalkable())
    //                Gizmos.color = Color.green;
    //            else
    //                Gizmos.color = Color.red;
    //            if (node == playerNode)
    //                Gizmos.color = Color.blue;
    //            Gizmos.DrawSphere(node.GetPosition(), nodeDistance / 2);
    //        }
    //    }
    //}
}
