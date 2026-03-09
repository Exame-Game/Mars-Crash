using System;

public class NodeButton : Node
{
    public event Action OnPressed;
    public event Action OnReleased;
    public bool IsPressed;

    public void Press()
    {
        IsPressed = true;
        Occupied = true;
        OnPressed?.Invoke();
    }

    public void Release()
    {
        IsPressed = false;
        Occupied = false;
        OnReleased?.Invoke();
    }
}