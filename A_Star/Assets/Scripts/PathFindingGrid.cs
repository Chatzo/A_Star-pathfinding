using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
    internal class PathFindingGrid : MonoBehaviour
    {
        [SerializeField] private Tile tilePrefab;
        private Node [,] nodeGrid;
        public int width;
        public int depth;
        public Color colorOne;
        public Color colorTwo;
        private void Start()
        {
            CreateGrid(width, depth);
        }

        private void CreateGrid(int width, int depth)
        {
            nodeGrid = new Node[width, depth];
            Tile tile;
            Node node;
            for (int i = 0; i < nodeGrid.GetLength(0); i++)
            {
                for(int j = 0; j < nodeGrid.GetLength(1); j++)
                {
                    tile = Instantiate(tilePrefab);
                    tile.transform.position = new Vector3(i, 0, j);
                    tile.transform.parent = this.transform;
                    node = new Node(tile);

                    if ((i % 2 != 0) == (j % 2 == 0))
                        tile.Init(node, colorTwo);
                    else
                        tile.Init(node, colorOne);
                    
                    nodeGrid[i, j] = node;
                }
            }
        }

        private void ResetColor(Color first, Color second)
        {
            for (int i = 0; i < nodeGrid.GetLength(0); i++)
            {
                for (int j = 0; j < nodeGrid.GetLength(1); j++)
                {
                    if ((i % 2 != 0) == (j % 2 == 0))
                        nodeGrid[i,j].tile.GetComponent<Renderer>().material.color = second;
                    else
                        nodeGrid[i, j].tile.GetComponent<Renderer>().material.color = first;
                }
            }
        }
        private void UpdateGrid()
        {

        }
    }
}
