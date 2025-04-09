using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Singleton instance of GameManager to access it globally
    public BoardManager BoardManager;
    public PlayerController PlayerController;
    public TurnManager TurnManager { get; private set; } // Reference to the TurnManager 

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject); // destroy the duplicate instance
            return;
        }
        Instance = this; // set the instance to this object
    }

    
    void Start()
    {
        TurnManager = new TurnManager();
        BoardManager.Init(); // Initialize the board manager
        PlayerController.Spawn(BoardManager, new Vector2Int(1, 1)); // Spawn the player at (1, 1)     
    }

}
