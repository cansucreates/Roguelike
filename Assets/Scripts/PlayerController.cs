using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private BoardManager m_Board; // Reference to the BoardManager
    private Vector2Int m_CellPosition; // The cell position of the player

    public void Spawn(BoardManager boardManager, Vector2Int cell)
    {
        m_Board = boardManager;
        m_CellPosition = cell;

        // Set the player's position to the center of the cell
        transform.position = m_Board.CellToWorld(cell);
    }
}
