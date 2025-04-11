using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WallObject : CellObject
{
    public Tile ObstacleTile; // Tile to be used for the wall object
    public int MaxHealth = 3;
    private int m_HealthPoint; // Health of the wall object
    private Tile m_OriginalTile; // Original tile of the wall object

    public override void Init(Vector2Int cell)
    {
        base.Init(cell);

        m_HealthPoint = MaxHealth; // Initialize the health point to the maximum health

        m_OriginalTile = GameManager.Instance.BoardManager.GetCellTile(cell); // Get the original tile of the cell
        GameManager.Instance.BoardManager.SetCellTile(cell, ObstacleTile); // Set the tile for the wall object in the board manager
    }

    public override bool PlayerWantsToEnter()
    {
        m_HealthPoint -= 1;

        if (m_HealthPoint > 0)
        {
            return false; // Return false to prevent the player from entering the wall object
        }

        GameManager.Instance.BoardManager.SetCellTile(m_Cell, m_OriginalTile); // Restore the original tile of the cell
        Destroy(gameObject); // Destroy the wall object
        return true; // Return true to allow the player to enter the cell
    }
}
