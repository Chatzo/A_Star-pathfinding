using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
    internal class TileGrid : MonoBehaviour
    {
        [SerializeField] private Tile tilePrefab;
        public Tile [,] tileGrid { get; private set; }
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
            tileGrid = new Tile[width, depth];
            Tile tile;
            for (int i = 0; i < tileGrid.GetLength(0); i++)
            {
                for(int j = 0; j < tileGrid.GetLength(1); j++)
                {
                    tile = Instantiate(tilePrefab);
                    tile.transform.position = new Vector3(i, 0, j);
                    tile.transform.parent = this.transform;

                    if ((i % 2 != 0) == (j % 2 == 0))
                        tile.Init(colorTwo);
                    else
                        tile.Init(colorOne);
                    
                    tileGrid[i, j] = tile;
                }
            }
        }

        private void ResetColor(Color first, Color second)
        {
            for (int i = 0; i < tileGrid.GetLength(0); i++)
            {
                for (int j = 0; j < tileGrid.GetLength(1); j++)
                {
                    if ((i % 2 != 0) == (j % 2 == 0))
                        tileGrid[i,j].GetComponent<Renderer>().material.color = second;
                    else
                        tileGrid[i, j].GetComponent<Renderer>().material.color = first;
                }
            }
        }
        private void UpdateGrid()
        {

        }
    }
}
