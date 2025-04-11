using UnityEngine;

public class FoodObject : CellObject
{
    public int FoodAmount = 10; // Amount of food to be added

    public override void PlayerEntered()
    {
        Destroy(gameObject); // Destroy the food object when the player enters
        Debug.Log("Food increased!"); // Log the event
        GameManager.Instance.ChangeFood(FoodAmount); // Call the ChangeFood method in GameManager to increase food count
    }
}
