using Unity.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    // for to get cell data
    public class CellData
    {
        public bool Passable;
    }

    private Tilemap m_Tilemap; // used m cause it's private

    public int Width;
    public int Height;
    public Tile[] GroundTiles;
    public Tile[] WallTiles;
    private CellData[,] m_BoardData; // 2D array to store cell data
    private Grid m_Grid; // Reference to the Grid component
    public PlayerController Player;

    public void Init() 
    {
        m_Tilemap = GetComponentInChildren<Tilemap>();
        m_BoardData = new CellData[Width, Height]; // Initialize the 2D array
        m_Grid = GetComponentInChildren<Grid>(); // Get the Grid component

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
                }

                m_Tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
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
}
