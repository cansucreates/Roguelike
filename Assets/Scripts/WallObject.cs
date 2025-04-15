using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WallObject : CellObject
{
    public Tile[] ObstacleTile; // Tile to be used for the wall object
    public Tile[] DamagedObstacleTiles; // Tile to be used for the damaged wall object
    private int m_SelectedTileIndex; // Index of the selected tile
    public int MaxHealth = 3;
    private int m_HealthPoint; // Health of the wall object
    private Tile m_OriginalTile; // Original tile of the wall object

    public override void Init(Vector2Int cell)
    {
        base.Init(cell);

        m_HealthPoint = MaxHealth; // Initialize the health point to the maximum health
        m_OriginalTile = GameManager.Instance.BoardManager.GetCellTile(cell); // Get the original tile of the cell

        m_SelectedTileIndex = Random.Range(0, ObstacleTile.Length); // Randomly select a tile index
        Tile tile = ObstacleTile[m_SelectedTileIndex]; // Get the selected tile
        GameManager.Instance.BoardManager.SetCellTile(cell, tile); // Set the tile for the wall object in the board manager
    }

    public override bool PlayerWantsToEnter(PlayerController player)
    {
        m_HealthPoint -= 1;

        if (m_HealthPoint == 1)
        {
            Tile damagedTile = DamagedObstacleTiles[m_SelectedTileIndex]; // Get the damaged tile
            GameManager.Instance.BoardManager.SetCellTile(m_Cell, damagedTile); // Set the tile for the wall object in the board manager
            player.PlayAttackAnimation(); // Play the attack animation for the player
            return false; // Return false to prevent the player from entering the wall object
        }

        if (m_HealthPoint > 0)
        {
            player.PlayAttackAnimation(); // Play the attack animation for the player
            return false; // Return false to prevent the player from entering the wall object
        }

        GameManager.Instance.BoardManager.SetCellTile(m_Cell, m_OriginalTile); // Restore the original tile of the cell
        Destroy(gameObject); // Destroy the wall object
        return true;
    }
}
