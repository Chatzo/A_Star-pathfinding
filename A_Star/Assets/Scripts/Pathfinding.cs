using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class Pathfinding : MonoBehaviour
{
    private NodeGrid nodeGrid;
    private List<Node> openList;
    private List<Node> closedList;
    private List<Node> actualPath;
    private int movementCost = 10;

    private bool targetFound = false;
    private void Start()
    {
        openList = new List<Node>();
        closedList = new List<Node>();
        actualPath = new List<Node>();
        nodeGrid = FindObjectOfType<NodeGrid>();
    }
    internal List<Node> GetPath(Vector3 startPosition, Vector3 targetPostion)
    {
        Node startNode = nodeGrid.GetNodeFromWorldPosition(startPosition);
        Node targetNode = nodeGrid.GetNodeFromWorldPosition(targetPostion);
        return GetPath(startNode, targetNode);
    }
    private List<Node> GetPath(Node startNode, Node targetNode)
    {
        actualPath.Clear();
        Reset();
        if (IsNodeValid(startNode) && IsNodeValid(targetNode) && startNode != targetNode)
        {
            openList.Add(startNode);
            int hCost = GetHeuristic(startNode, targetNode);
            startNode.SetHCost(hCost);
            FindPath(startNode, targetNode);
            
        }
        return actualPath;
    }

    private void FindPath(Node currentNode, Node targetNode)
    {
        if (openList.Contains(currentNode))
        {
            openList.Remove(currentNode);
            closedList.Add(currentNode);
        }
        ProcessAllNeighbours(currentNode, targetNode);
        if (targetFound == false)
        {
            Node lowestFValue = GetNodeWithLowestFValueFromOpenList();
            FindPath(lowestFValue, targetNode);
        }
    }

    private void ProcessAllNeighbours(Node currentNode, Node targetNode)
    {
        Node[] neighbours = nodeGrid.GetNeighbours(currentNode);
        bool isDiagonal = false;
        for (int i = 0; i < neighbours.Length; i++)
        {
            //0,1,2
            //3,X,4
            //5,6,7
            if (i == 0 || i == 2 || i == 5 || i == 7)
                isDiagonal = true;
            else
                isDiagonal = false;
            if (neighbours[i] == null || currentNode == null || targetNode == null)
            {
                Debug.Log(neighbours[i] + " = null");
                Debug.Log(currentNode + " = null");
                Debug.Log(targetNode + " = null");
            }
            ProcessNeighbour(neighbours[i], currentNode, targetNode, isDiagonal);
            if (targetFound)
                return;
        }
    }

    private void ProcessNeighbour(Node neighbour, Node currentNode, Node targetNode, bool isDiagonal)
    {
        if (IsNodeValid(neighbour) && closedList.Contains(neighbour) == false && openList.Contains(neighbour) == false)
        {
            neighbour.SetPathParent(currentNode);
            if (neighbour == targetNode)
            {
                targetFound = true;
                closedList.Add(neighbour);
                CreateActualPath(neighbour);
                return;
            }

            ProcessGValueAndHValues(neighbour, currentNode, targetNode, isDiagonal);
            //fCost is gCost + hCost and is added in each node.
            openList.Add(neighbour);
        }
        else if (openList.Contains(neighbour) /*|| closedList.Contains(neighbour)*/)
        {
            ProcessGValueAndHValues(neighbour, currentNode, targetNode, isDiagonal);
            if(isDiagonal && currentNode.gCost + (int)(movementCost* 1.401f) < neighbour.gCost)
            {
                neighbour.SetPathParent(currentNode);
            }
            else if (isDiagonal == false && currentNode.gCost + movementCost < neighbour.gCost)
            {
                neighbour.SetPathParent(currentNode);
            }
        }
    }

    private void ProcessGValueAndHValues(Node neighbour, Node currentNode, Node targetNode, bool isDiagonal)
    {
        neighbour.SetHCost(GetHeuristic(neighbour, targetNode));
        if (isDiagonal)
            neighbour.SetGCost(currentNode.gCost + (int)(movementCost * 1.4)); // 8 directional, diagonal cost is with pythagoras ~1.4 
        else
            neighbour.SetGCost(currentNode.gCost + movementCost); // 4 directional
    }

    private int GetHeuristic(Node node, Node targetNode)
    {
        int diagonalCost = (int)(movementCost * 1.401f); //diagonal distance with sides of 1 is with pythagoras ~1.4 *
        int deltaX = Mathf.Abs(node.GetGridIndex().x - targetNode.GetGridIndex().x);
        int deltaY = Mathf.Abs(node.GetGridIndex().y - targetNode.GetGridIndex().y);
        //for 4 directional movement
        //return (Mathf.Abs(deltaX) + Mathf.Abs(deltaY)) * movementCost;

        //for 8 directional movement
        if (deltaX > deltaY)
            return diagonalCost * deltaY + movementCost * (deltaX - deltaY);
        else
            return diagonalCost * deltaX + movementCost * (deltaY - deltaX);

    }
    private void CreateActualPath(Node p_Parent)
    {
        if (p_Parent != null)
        {
            actualPath.Add(p_Parent);
            CreateActualPath(p_Parent.pathParent);
        }
    }
    private Node GetNodeWithLowestFValueFromOpenList()
    {
        Node lowest = null;
        if (openList.Count > 0)
            lowest = openList[0];
        for (int i = 0; i < openList.Count; i++)
        {
            if (openList[i].fCost < lowest.fCost)
                lowest = openList[i];
        }
        return lowest;
    }
    private bool IsNodeValid(Node node)
    {
        return node != null && node.GetWalkable();
    }

    private void Reset()
    {
        targetFound = false;
        ResetNodes();
        openList.Clear();
        closedList.Clear();
    }
    private void ResetNodes()
    {
        for (int i = 0; i < openList.Count; i++)
        {
            openList[i].ResetNode();
        }
        for (int i = 0; i < closedList.Count; i++)
        {
            closedList[i].ResetNode();
        }
    }
    void OnDrawGizmos()
    {
        if (actualPath != null)
        {
            for (int i = 0; i < openList.Count; i++)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(openList[i].GetPosition(),nodeGrid.nodeDistance/2);
            }
            for (int i = 0; i < closedList.Count; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(closedList[i].GetPosition(), nodeGrid.nodeDistance / 2);
            }
            for (int i = 0; i < actualPath.Count; i++)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(actualPath[i].GetPosition(), nodeGrid.nodeDistance / 2);
            }
        }
    }
}
