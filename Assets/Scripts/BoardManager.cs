using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    // for to get cell data
    public class CellData
    {
        public bool Passable;
        public CellObject ContainedObject; // The health bject contained in the cell, if any
    }

    private Tilemap m_Tilemap; // used m cause it's private

    public int Width;
    public int Height;
    public Tile[] GroundTiles;
    public Tile[] WallTiles;
    private CellData[,] m_BoardData; // 2D array to store cell data
    private Grid m_Grid; // Reference to the Grid component
    public PlayerController Player;
    public FoodObject[] FoodPrefab; // Prefab for the food object
    private List<Vector2Int> m_EmptyCellsList;
    public int minFood; // Minimum number of food items to generate
    public int maxFood; // Maximum number of food items to generate
    public WallObject WallPrefab; // Prefab for the wall object
    public ExitCellObject ExitCellPrefab; // Prefab for the exit object
    public Enemy EnemyPrefab; // Prefab for the enemy object
    public int enemyCount; // Number of enemies to generate

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

        Vector2Int endCoord = new Vector2Int(Width - 2, Height - 2); // Set the exit cell coordinates
        AddObject(Instantiate(ExitCellPrefab), endCoord); // Instantiate and add the exit cell object
        m_EmptyCellsList.Remove(endCoord); // Remove the exit cell from the empty cells list

        GenerateWall(); // Generate walls on the board
        GenerateFood(); // Generate food items on the board
        GenerateEnemy(); // Generate enemies on the board
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

            FoodObject randomFood = FoodPrefab[Random.Range(0, FoodPrefab.Length)]; // Randomly select a food prefab
            FoodObject newFood = Instantiate(randomFood); // Instantiate the food prefab
            AddObject(newFood, coord); // Add the food object to the cell
        }
    }

    void GenerateEnemy()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            int randomIndex = Random.Range(0, m_EmptyCellsList.Count); // Get a random index from the empty cells list
            Vector2Int coord = m_EmptyCellsList[randomIndex]; // Get a random cell from the empty cells list

            m_EmptyCellsList.RemoveAt(randomIndex); // Remove the cell from the empty cells list to avoid duplicates

            Enemy enemy = Instantiate(EnemyPrefab); // Instantiate the enemy prefab
            AddObject(enemy, coord); // Add the enemy object to the cell
        }
    }

    void GenerateWall()
    {
        int wallCount = Random.Range(6, 10); // Get a random number of walls to generate
        for (int i = 0; i < wallCount; i++)
        {
            int RandomIndex = Random.Range(0, m_EmptyCellsList.Count); // Get a random index from the empty cells list
            Vector2Int coord = m_EmptyCellsList[RandomIndex]; // Get a random cell from the empty cells list

            m_EmptyCellsList.RemoveAt(RandomIndex); // Remove the cell from the empty cells list to avoid duplicates

            WallObject newWall = Instantiate(WallPrefab); // Instantiate the wall prefab
            AddObject(newWall, coord); // Add the wall object to the cell
        }
    }

    public void SetCellTile(Vector2Int cellIndex, Tile tile)
    {
        m_Tilemap.SetTile(new Vector3Int(cellIndex.x, cellIndex.y, 0), tile); // Set the tile at the specified cell index
    }

    void AddObject(CellObject obj, Vector2Int coord)
    {
        CellData data = m_BoardData[coord.x, coord.y]; // Get the cell data
        obj.transform.position = CellToWorld(coord); // Set the position of the object
        data.ContainedObject = obj; // Store the object in the cell data
        obj.Init(coord); // Initialize the object with the cell coordinates
    }

    public Tile GetCellTile(Vector2Int cellIndex)
    {
        return m_Tilemap.GetTile(new Vector3Int(cellIndex.x, cellIndex.y, 0)) as Tile; // Get the tile at the specified cell index
    }

    public void Clean() // Clean the board
    {
        // no board data, so exit early, nothing to clean
        if (m_BoardData == null)
            return;

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                var cellData = m_BoardData[x, y];
                if (cellData.ContainedObject != null)
                {
                    Destroy(cellData.ContainedObject.gameObject); // Destroy the contained object
                    // destroy the gameobject not just cellData.ContainedObject otherwise
                    // what you are destyroying is just the CellObject component and not the whole
                    // gameobject with the sprite
                }
                SetCellTile(new Vector2Int(x, y), null); // Reset the tile to null
            }
        }
    }
}
