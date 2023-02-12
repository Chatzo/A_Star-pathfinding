using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

internal class Pathfinding : MonoBehaviour
{
    private NodeGrid nodeGrid;
    private List<Node> openList;
    private HashSet<Node> closedList;
    private List<Node> actualPath;
    private int movementCost = 10;
    private bool targetFound = false;
    private void Start()
    {
        openList = new List<Node>();
        closedList = new HashSet<Node>();
        actualPath = new List<Node>();
        nodeGrid = FindObjectOfType<NodeGrid>();
    }
    internal List<Node> GetPath(Vector3 startPosition, Vector3 targetPostion)
    {
        Stopwatch watch = new Stopwatch();
        actualPath.Clear();
        Node startNode = nodeGrid.GetNodeFromWorldPosition(startPosition);
        Node targetNode = nodeGrid.GetNodeFromWorldPosition(targetPostion);
        if (IsNodeValid(startNode) && IsNodeValid(targetNode) && startNode != targetNode)
        {
            watch.Start();
            openList.Add(startNode);
            FindPath(startNode, targetNode);
            print("Path found in: " + watch.ElapsedMilliseconds + " ms");
        }
        ResetAllNodes();
        return actualPath;
    }
    private void FindPath(Node currentNode, Node targetNode)
    {
        if (openList.Contains(currentNode))
        {
            openList.Remove(currentNode);
            closedList.Add(currentNode);
        }
        if (currentNode == targetNode)
        {
            targetFound = true;
            CreateActualPath(currentNode);
            return;
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
        for (int i = 0; i < neighbours.Length; i++)
        {
            ProcessNeighbour(neighbours[i], currentNode, targetNode);
        }
    }

    private void ProcessNeighbour(Node neighbour, Node currentNode, Node targetNode)
    {
        if (IsNodeValid(neighbour) && closedList.Contains(neighbour) == false)
        {
            int costToNeighbour = currentNode.gCost + GetHeuristic(currentNode, neighbour);
            if (costToNeighbour < neighbour.gCost || openList.Contains(neighbour) == false)
            {
                neighbour.SetGCost(costToNeighbour);
                neighbour.SetHCost(GetHeuristic(neighbour, targetNode));
                neighbour.SetPathParent(currentNode);

                if (openList.Contains(neighbour) == false)
                    openList.Add(neighbour);
            }
        }
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
    private void CreateActualPath(Node node)
    {
        if (node != null)
        {
            actualPath.Add(node);
            CreateActualPath(node.pathParent);
        }
    }
    private Node GetNodeWithLowestFValueFromOpenList()
    {
        Node lowest = null;
        if (openList.Count > 0)
            lowest = openList[0];
        for (int i = 1; i < openList.Count; i++)
        {
            if (openList[i].fCost < lowest.fCost || openList[i].fCost == lowest.fCost && openList[i].hCost < lowest.hCost)
                lowest = openList[i];
        }
        return lowest;
    }
    private bool IsNodeValid(Node node)
    {
        return node != null && node.GetWalkable();
    }

    private void ResetAllNodes()
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
        //for (int i = 0; i < closedList.Count; i++)
        //{
        //    closedList[i].ResetNode();
        //}
        foreach(Node node in closedList)
        {
            node.ResetNode();
        }
    }
    void OnDrawGizmos()
    {
        if (actualPath != null)
        {
            for (int i = 0; i < openList.Count; i++)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(openList[i].GetPosition(), nodeGrid.nodeDistance / 2);
            }
            //for (int i = 0; i < closedList.Count; i++)
            //{
            //    Gizmos.color = Color.red;
            //    Gizmos.DrawSphere(closedList[i].GetPosition(), nodeGrid.nodeDistance / 2);
            //}
            foreach(Node node in closedList)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(node.GetPosition(), nodeGrid.nodeDistance / 2);
            }
            for (int i = 0; i < actualPath.Count; i++)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(actualPath[i].GetPosition(), nodeGrid.nodeDistance / 2);
            }
        }
    }
}
