using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Singleton instance of GameManager to access it globally
    public BoardManager BoardManager;
    public PlayerController PlayerController;
    public TurnManager TurnManager { get; private set; } // Reference to the TurnManager 
    private int m_CurrentFood = 100; // Current food count

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
        TurnManager.onTick += OnTurnHappen; // Subscribe to the onTick event


        BoardManager.Init(); // Initialize the board manager
        PlayerController.Spawn(BoardManager, new Vector2Int(1, 1)); // Spawn the player at (1, 1)     
    }

    public void OnTurnHappen() {
        m_CurrentFood -= 1; // decrease food count by 1 for each turn
        Debug.Log("Current Food: " + m_CurrentFood);
        if (m_CurrentFood <= 0) {
            Debug.Log("Game Over!"); // Game over if food count is 0
        }
    }

}
