using UnityEngine;

public class TurnManager
{
    public event System.Action onTick; // Event to notify when a turn is ticked
    private int m_TurnCount;

    public TurnManager() {
        m_TurnCount = 1;
    }

    public void Tick() {
        onTick?.Invoke(); // invoke the event
        m_TurnCount += 1;
        Debug.Log("Turn: " + m_TurnCount);
    }
}
