using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGrid : MonoBehaviour
{
    private Node[,] nodeGrid;
    private LayerMask unwalkable;
    public Vector2 gridSize;
    public float nodeDistance;
    private void Start()
    {
        unwalkable = LayerMask.GetMask("Obstacle");
        int width = Mathf.RoundToInt(gridSize.x / nodeDistance);
        int depth = Mathf.RoundToInt(gridSize.y / nodeDistance);
        CreateGrid(width, depth);
    }
    private void CreateGrid(int width, int depth)
    {
        nodeGrid = new Node[width, depth];
        float nodeRadius = nodeDistance / 2f;
        Vector3 xDirection = new Vector3(gridSize.x / 2, 0f, 0f);
        Vector3 yDirection = new Vector3(0f, 0f, gridSize.y / 2);
        Vector3 bottomLeft = transform.position - xDirection - yDirection;
        Vector3 worldPosition;
        Vector3 currentX;
        Vector3 currentY;
        
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < depth; j++)
            {
                currentX = Vector3.right * (i * nodeDistance + nodeRadius);
                currentY = Vector3.forward * (j * nodeDistance + nodeRadius);
                worldPosition = bottomLeft + currentX + currentY;
                bool walkable = true;
                if (Physics.CheckSphere(worldPosition, nodeRadius, unwalkable))
                    walkable = false;
                nodeGrid[i, j] = new Node(worldPosition, walkable);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 nodeVector = new Vector3(gridSize.x, 1, gridSize.y);
        Gizmos.DrawWireCube(transform.position, nodeVector);

        if(nodeGrid != null)
        {
            foreach(Node node in nodeGrid)
            {
                if (node.GetWalkable())
                    Gizmos.color = Color.green;
                else
                    Gizmos.color = Color.red;
                Gizmos.DrawSphere(node.GetPosition(), nodeDistance/2);
            }
        }
    }
}
