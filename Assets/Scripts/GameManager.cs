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
        m_FoodLabel = UIDoc.rootVisualElement.Q<Label>("FoodLabel"); // Get the food label from the UI Document
        m_FoodLabel.text = "Food: " + m_CurrentFood; // Set the initial food count in the UI

        TurnManager = new TurnManager();
        TurnManager.onTick += OnTurnHappen; // Subscribe to the onTick event

        BoardManager.Init(); // Initialize the board manager
        PlayerController.Spawn(BoardManager, new Vector2Int(1, 1)); // Spawn the player at (1, 1)
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
    }
}
