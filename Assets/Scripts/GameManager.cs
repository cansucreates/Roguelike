using UnityEngine;
using UnityEngine.UIElements; // For UI Document

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Singleton instance of GameManager to access it globally
    public BoardManager BoardManager;
    public PlayerController PlayerController;
    public TurnManager TurnManager { get; private set; } // Reference to the TurnManager
    private int m_CurrentFood = 100; // Current food count
    public UIDocument UIDoc; // Reference to the UI Document
    private Label m_FoodLabel; // Reference to the food label in the UI
    private int m_CurrentLevel = 0; // Current level of the game
    private VisualElement m_GameOverPanel; // Reference to the game over panel in the UI
    private Label m_GameOverMessage; // Reference to the game over message label in the UI

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject); // destroy the duplicate instance
            return;
        }
        Instance = this; // set the instance to this object
    }

    void Start()
    {
        m_GameOverPanel = UIDoc.rootVisualElement.Q<VisualElement>("GameOverPanel"); // Get the game over panel from the UI Document
        m_GameOverMessage = m_GameOverPanel.Q<Label>("GameOverMessage"); // Get the game over message label from the UI Document

        m_GameOverPanel.style.visibility = Visibility.Hidden; // Hide the game over panel at the start

        TurnManager = new TurnManager();
        TurnManager.onTick += OnTurnHappen; // Subscribe to the onTick event

        NewLevel(); // Start a new level

        m_FoodLabel = UIDoc.rootVisualElement.Q<Label>("FoodLabel"); // Get the food label from the UI Document
        m_FoodLabel.text = "Food: " + m_CurrentFood; // Set the initial food count in the UI
    }

    public void OnTurnHappen() // method to handle the turn event
    {
        ChangeFood(-1); // Decrease food count by 1 each turn
    }

    // Method to add food to the current food count
    public void ChangeFood(int food)
    {
        m_CurrentFood += food; // Increase the food count
        m_FoodLabel.text = "Food: " + m_CurrentFood; // Update the food count in the UI

        if (m_CurrentFood <= 0) // If food count is less than or equal to 0
        {
            m_GameOverPanel.style.visibility = Visibility.Visible; // Show the game over panel
            m_GameOverMessage.text =
                "Game Over!\n\nYou traveled through " + m_CurrentLevel + " levels"; // Set the game over message
        }
    }

    public void NewLevel()
    {
        BoardManager.Clean(); // Clean the board for a new level
        BoardManager.Init(); // Initialize the board manager for the new level
        PlayerController.Spawn(BoardManager, new Vector2Int(1, 1)); // Spawn the player at (1, 1) for the new level
        m_CurrentLevel++; // Increase the level count
    }
}
