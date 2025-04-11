using UnityEngine;

public class CellObject : MonoBehaviour
{
    protected Vector2Int m_Cell; // The cell position of the object

    public virtual void Init(Vector2Int cell)
    {
        m_Cell = cell; // Initialize the cell position
    }

    public virtual void PlayerEntered() { }

    public virtual bool PlayerWantsToEnter()
    {
        return true; // Return true to allow the player to enter the cell
    }
}
