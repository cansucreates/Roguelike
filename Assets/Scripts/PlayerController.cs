using UnityEngine;
using UnityEngine.InputSystem;

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

    private void Update()
    {
        Vector2Int newCellTarget = m_CellPosition;
        bool hasMoved = false;

        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            newCellTarget.y += 1;
            hasMoved = true;
        }
        else if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            newCellTarget.y -= 1;
            hasMoved = true;
        }
        else if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            newCellTarget.x -= 1;
            hasMoved = true;
        }
        else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            newCellTarget.x += 1;
            hasMoved = true;
        }

        if (hasMoved)
        {
            // check if the cell is passable
            BoardManager.CellData cellData = m_Board.GetCellData(newCellTarget);

            if (cellData != null && cellData.Passable)
            {
                m_CellPosition = newCellTarget; // Update the cell position
                // Move the player to the new cell position
                transform.position = m_Board.CellToWorld(m_CellPosition);
            }
        }
    }
}
