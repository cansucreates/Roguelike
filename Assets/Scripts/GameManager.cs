using UnityEngine;

public class GameManager : MonoBehaviour
{
    public BoardManager BoardManager;
    public PlayerController PlayerController;
    private TurnManager m_TurnManager;

    
    void Start()
    {
        m_TurnManager = new TurnManager();
        BoardManager.Init(); // Initialize the board manager
        PlayerController.Spawn(BoardManager, new Vector2Int(1, 1)); // Spawn the player at (1, 1)     
    }

 
    void Update()
    {
        
    }
}
