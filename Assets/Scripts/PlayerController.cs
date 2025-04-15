using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private BoardManager m_Board; // Reference to the BoardManager
    private Vector2Int m_CellPosition; // The cell position of the player
    private bool m_IsGameOver; // Flag to check if the game is over
    private bool m_IsMoving; // Flag to check if the player is moving
    private Vector3 m_MoveTarget; // The target position for the player to move to
    public float MoveSpeed = 5f; // Speed of the player movement
    private Animator m_Animator; // Reference to the Animator component

    void Awake()
    {
        m_Animator = GetComponent<Animator>(); // Get the Animator component attached to the player
    }

    public void Spawn(BoardManager boardManager, Vector2Int cell)
    {
        m_Board = boardManager;
        m_CellPosition = cell; // Set the initial cell position of the player
        transform.position = m_Board.CellToWorld(cell); // Set the initial position of the player in world space
        m_IsMoving = false; // Initialize the moving flag to false
    }

    // the method for moving the player
    public void MoveTo(Vector2Int cell)
    {
        m_CellPosition = cell;

        m_IsMoving = true; // Set the moving flag to true
        m_MoveTarget = m_Board.CellToWorld(m_CellPosition); // Get the target position in world space

        m_Animator.SetBool("Moving", m_IsMoving); // Set the animator parameter to trigger the moving animation
    }

    public void Init()
    {
        m_IsMoving = false; // Initialize the moving flag to false
        m_IsGameOver = false; // Initialize the game over flag to false
    }

    private void Update()
    {
        Vector2Int newCellTarget = m_CellPosition;
        bool hasMoved = false;

        if (m_IsGameOver)
        {
            if (Keyboard.current.enterKey.wasPressedThisFrame)
            {
                GameManager.Instance.StartNewGame();
            }

            return;
        }

        if (m_IsMoving)
        {
            // Move the player towards the target position
            transform.position = Vector3.MoveTowards(
                transform.position,
                m_MoveTarget,
                MoveSpeed * Time.deltaTime
            );

            if (transform.position == m_MoveTarget)
            {
                m_IsMoving = false; // Stop moving when the target position is reached
                m_Animator.SetBool("Moving", false); // Set the animator parameter to stop the moving animation
                var CellData = m_Board.GetCellData(m_CellPosition);
                if (CellData.ContainedObject != null)
                {
                    CellData.ContainedObject.PlayerEntered(); // Call the PlayerEntered method on the contained object
                }
            }
        }

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
                }
            }
        }
    }

    public void GameOver()
    {
        m_IsGameOver = true;
    }
}
