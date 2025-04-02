using Unity.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    private Tilemap m_Tilemap;

    public int Width;
    public int Height;
    public Tile[] GroundTiles;
    public Tile[] WallTiles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_Tilemap = GetComponentInChildren<Tilemap>();
        
        // Create the border and fill the map with ground tiles
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Tile tile;

                if (x == 0 || y == 0 || x == Width - 1 || y == Height - 1) // If we are on the border of the map
                {
                    tile = WallTiles[Random.Range(0, WallTiles.Length)]; // Randomly select a wall tile
                }
                else
                {
                    tile = GroundTiles[Random.Range(0, GroundTiles.Length)]; // Randomly select a ground tile
                }

                m_Tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
    }

    // Update is called once per frame
    void Update() { }
}
