using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    // for to get cell data
    public class CellData
    {
        public bool Passable;
        public GameObject ContainedObject; // The health bject contained in the cell, if any
    }

    private Tilemap m_Tilemap; // used m cause it's private

    public int Width;
    public int Height;
    public Tile[] GroundTiles;
    public Tile[] WallTiles;
    private CellData[,] m_BoardData; // 2D array to store cell data
    private Grid m_Grid; // Reference to the Grid component
    public PlayerController Player;
    public GameObject[] FoodPrefab; // Prefab for the food object
    private List<Vector2Int> m_EmptyCellsList;
    public int minFood; // Minimum number of food items to generate
    public int maxFood; // Maximum number of food items to generate

    public void Init()
    {
        m_Tilemap = GetComponentInChildren<Tilemap>();
        m_Grid = GetComponentInChildren<Grid>(); // Get the Grid component
        m_EmptyCellsList = new List<Vector2Int>(); // Initialize the empty cells list
        m_BoardData = new CellData[Width, Height]; // Initialize the 2D array

        // Create the border and fill the map with ground tiles
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Tile tile;
                m_BoardData[x, y] = new CellData();

                if (x == 0 || y == 0 || x == Width - 1 || y == Height - 1) // If we are on the border of the map
                {
                    tile = WallTiles[Random.Range(0, WallTiles.Length)]; // Randomly select a wall tile
                    m_BoardData[x, y].Passable = false;
                }
                else
                {
                    tile = GroundTiles[Random.Range(0, GroundTiles.Length)]; // Randomly select a ground tile
                    m_BoardData[x, y].Passable = true;
                    m_EmptyCellsList.Add(new Vector2Int(x, y)); // Add the cell to the empty cells list
                }

                m_Tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }

        m_EmptyCellsList.Remove(new Vector2Int(1, 1)); // Remove the starting position from the empty cells list cause player will spawn there
        GenerateFood(); // Generate food items on the board
    }

    // transform cell index to world position
    public Vector3 CellToWorld(Vector2Int cellIndex)
    {
        return m_Grid.GetCellCenterWorld((Vector3Int)cellIndex);
    }

    // the method for getting the cell data
    public CellData GetCellData(Vector2Int cellIndex)
    {
        if (cellIndex.x < 0 || cellIndex.x >= Width || cellIndex.y < 0 || cellIndex.y >= Height)
        {
            return null;
        }
        return m_BoardData[cellIndex.x, cellIndex.y];
    }

    void GenerateFood()
    {
        int randomFoodCount = Random.Range(minFood, maxFood + 1); // Get a random number of food items to generate
        int foodCount = Mathf.Min(randomFoodCount, m_EmptyCellsList.Count); // Ensure we don't exceed the number of empty cells

        for (int i = 0; i < foodCount; i++)
        {
            int randomIndex = Random.Range(0, m_EmptyCellsList.Count); // Get a random index from the empty cells list
            Vector2Int coord = m_EmptyCellsList[randomIndex]; // Get a random cell from the empty cells list

            m_EmptyCellsList.RemoveAt(randomIndex); // Remove the cell from the empty cells list to avoid duplicates
            CellData data = m_BoardData[coord.x, coord.y]; // Get the cell data

            GameObject randomFood = FoodPrefab[Random.Range(0, FoodPrefab.Length)]; // Randomly select a food prefab
            GameObject newFood = Instantiate(randomFood); // Instantiate the food prefab
            newFood.transform.position = CellToWorld(coord); // Set the position of the food object
            data.ContainedObject = newFood; // Store the food object in the cell data
        }
    }
}
