using UnityEngine;

public class FoodObject : CellObject
{
    public override void PlayerEntered()
    {
        Destroy(gameObject); // Destroy the food object when the player enters
        Debug.Log("Food increased!"); // Log the event
    }
}
