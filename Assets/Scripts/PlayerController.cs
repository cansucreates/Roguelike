using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private BoardManager m_Board; // Reference to the BoardManager
    private Vector2Int m_CellPosition; // The cell position of the player

    public void Spawn(BoardManager boardManager, Vector2Int cell)
    {
        m_Board = boardManager;
        MoveTo(cell); // Move the player to the specified cell
    }

    // the method for moving the player
    public void MoveTo(Vector2Int cell)
    {
        m_CellPosition = cell;
        transform.position = m_Board.CellToWorld(m_CellPosition);
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
                GameManager.Instance.TurnManager.Tick(); // Call the TurnManager to tick the turn
                if (cellData.ContainedObject == null)
                {
                    MoveTo(newCellTarget); // Move the player to the new cell
                }
                else if (cellData.ContainedObject.PlayerWantsToEnter())
                {
                    MoveTo(newCellTarget); // Move the player to the new cell
                    cellData.ContainedObject.PlayerEntered(); // Call the PlayerEntered method on the contained object
                }
            }
        }
    }
}
